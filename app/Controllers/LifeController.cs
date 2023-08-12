using ActuarialApplications.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ActuarialApplications.Controllers;

public class LifeController : Controller
{
    // HttpClient lifecycle management best practices:
    // https://learn.microsoft.com/dotnet/fundamentals/networking/http/httpclient-guidelines#recommended-use
    private static HttpClient _sharedClient;

    private readonly string _belUri;

    private readonly AirflowDbContext _context;
    private DateTime _selectedDate;

    public LifeController(AirflowDbContext context, IConfiguration config)
    {
        _context = context;
        _belUri = config.GetValue<string>("Serverless:life");
        _sharedClient = new HttpClient
        {
            BaseAddress = new Uri(_belUri)
        };
    }

    // GET: Contracts
    public async Task<IActionResult> Index([Bind("SelectedContractNo")] int selectedContractNo)
    {
        var contractNoList = new SelectList(await _context.Contracts.Select(c => c.ContractNo).Distinct()
            .OrderByDescending(s => s).ToListAsync());
        if (selectedContractNo == 0)
            selectedContractNo = _context.Contracts.Select(c => c.ContractNo).DefaultIfEmpty().Max();

        Contract selectedContract = await _context.Contracts.FindAsync(selectedContractNo);
        _selectedDate = DateTime.SpecifyKind(selectedContract.ValueDate, DateTimeKind.Utc);
        List<RiskFreeRateData> rfr = await GetDiscountAsync(_selectedDate);
        List<CashFlow> cf = await GetCashFlowsAsync(selectedContract);
        List<CashFlow> discountedCashFlows = GetDiscountedCashFlows(cf, rfr);
        var indexModel = new LifeIndexModel
        {
            contractNoList = contractNoList,
            SelectedContractNo = selectedContractNo,
            contract = selectedContract,
            Age = GetAge(selectedContract),
            TechnicalProvision = GetTechnicalProvision(discountedCashFlows),
            cashFlows = cf,
            discountedCashFlows = discountedCashFlows
        };

        return View(indexModel);
    }

    // GET: Life/Create [Cashflows]
    public async Task<IActionResult> Create(
        [Bind("ValueDate,ContractNo,BirthDate,Sex,VestingAge,GuaranteeBenefit,PayPeriod,Table")]
        Contract contract)
    {
        using var response = await _sharedClient.PostAsJsonAsync("cashflows/", contract);
        var cf = await response.Content.ReadFromJsonAsync<List<double>>();
        // Necessary for postgresql to recognize the date as UTC
        var utcDate = DateTime.SpecifyKind(contract.ValueDate, DateTimeKind.Utc);

        var cashflows = cf.Select((value, index) => new CashFlow
        {
            ContractNo = contract.ContractNo,
            ValueDate = utcDate,
            Month = index,
            Benefit = value
        }).ToList();
        if (ModelState.IsValid)
        {
            if (CashFlowExists(cashflows.First().ValueDate, cashflows.First().ContractNo))
            {
                _context.UpdateRange(cashflows);
                TempData["Message"] = "Cashflows Updated";
            }
            else
            {
                _context.AddRange(cashflows);
                TempData["Message"] = "Cashflows Inserted";
            }

            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool CashFlowExists(DateTime valueDate, int contractNo)
    {
        return _context.CashFlows.Any(e => e.ValueDate == valueDate && e.ContractNo == contractNo);
    }

    private double GetAge(Contract contract)
    {
        var age = contract.ValueDate - contract.BirthDate;
        return age.Days / 365.25;
    }

    private async Task<List<CashFlow>> GetCashFlowsAsync(Contract contract)
    {
        var cashFlows = await _context.CashFlows.Where(c => c.ContractNo == contract.ContractNo).OrderBy(c => c.Month)
            .ToListAsync();
        return cashFlows;
    }

    private async Task<List<RiskFreeRateData>> GetDiscountAsync(DateTime valueDate)
    {
        int projId = await _context.RiskFreeRates.Where(r => r.ValueDate == valueDate).Select(r => r.ProjectionId)
            .FirstOrDefaultAsync();
        var riskFreeRateData = await _context.RiskFreeRateData.Where(r => r.ProjectionId == projId)
            .OrderBy(r => r.Month).ToListAsync();
        return riskFreeRateData;
    }

    private List<CashFlow> GetDiscountedCashFlows(List<CashFlow> cashFlows, List<RiskFreeRateData> riskFreeRateData)
    {
        var discountedCashFlows = new List<CashFlow>();
        for (int i = 0; i < cashFlows.Count; i++)
        {
            var discountedCashFlow = new CashFlow
            {
                ContractNo = cashFlows[i].ContractNo,
                ValueDate = cashFlows[i].ValueDate,
                Month = cashFlows[i].Month,
                Benefit = cashFlows[i].Benefit * riskFreeRateData[i].Price
            };
            discountedCashFlows.Add(discountedCashFlow);
        }

        return discountedCashFlows;
    }
    
    private double GetTechnicalProvision(List<CashFlow> discountedCashFlows)
    {
        return discountedCashFlows.Sum(c => c.Benefit);
    }
}