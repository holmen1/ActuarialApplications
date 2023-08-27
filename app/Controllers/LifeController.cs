using ActuarialApplications.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ActuarialApplications.Controllers;

public class LifeController : Controller
{
    private readonly AirflowDbContext _context;
    private readonly IHttpClientFactory _clientFactory;
    private readonly IConfiguration _config;

    private DateTime _selectedDate;

    public LifeController(AirflowDbContext context, IHttpClientFactory clientFactory, IConfiguration config)
    {
        _context = context;
        _clientFactory = clientFactory;
        _config = config;
    }

    // GET: Contracts
    public async Task<IActionResult> Index([Bind("SelectedContractNo")] int selectedContractNo)
    {
        _selectedDate = await GetValueDateAsync();
        var contractNoList = new SelectList(await _context.Contracts.Where(c => c.ValueDate == _selectedDate)
            .Select(c => c.ContractNo).Distinct().OrderBy(n => n).ToListAsync());

        if (selectedContractNo == 0)
            selectedContractNo = _context.Contracts.Where(c => c.ValueDate == _selectedDate).Select(c => c.ContractNo)
                .Max();

        var selectedContract = await GetContractAsync(_selectedDate, selectedContractNo);

        var rfr = await GetDiscountAsync(_selectedDate);
        var cf = await GetCashFlowsAsync(selectedContract);
        var discountedCashFlows = GetDiscountedCashFlows(cf, rfr);
        var indexModel = new LifeIndexModel
        {
            ContractNoList = contractNoList,
            SelectedContractNo = selectedContractNo,
            Contract = selectedContract,
            Age = GetAge(selectedContract),
            TechnicalProvision = GetTechnicalProvision(discountedCashFlows),
            CashFlows = cf,
            DiscountedCashFlows = discountedCashFlows
        };

        return View(indexModel);
    }

    // GET: Life/Create [Cashflows]
    public async Task<IActionResult> Create(
        [Bind("ValueDate,ContractNo,BirthDate,Sex,VestingAge,GuaranteeBenefit,PayPeriod,Table")]
        Contract contract)
    {
        var uri = _config.GetValue<string>("Serverless:life") + "cashflows/";
        var client = _clientFactory.CreateClient();
        using var response = await client.PostAsJsonAsync(uri, contract);
        var cf = await response.Content.ReadFromJsonAsync<List<ResponseCashFlow>>();

        // Necessary for postgresql to recognize the date as UTC
        var utcDate = DateTime.SpecifyKind(contract.ValueDate, DateTimeKind.Utc);//.ToUniversalTime();

        double minBenefit = 1E-12;
        var cashflows = cf.Select(c => new CashFlow
        {
            ContractNo = contract.ContractNo,
            ValueDate = utcDate,
            Month = c.Month,
            // Handles System.OverflowException: Numeric value does not fit in a System.Decimal
            Benefit = Convert.ToDouble(c.Benefit) < minBenefit ? minBenefit : c.Benefit
        }).ToList();
        if (ModelState.IsValid)
        {
            if (CashFlowExists(utcDate, contract.ContractNo))
            {
                var cashflowsToDelete = await _context.CashFlows.Where(c => c.ValueDate == utcDate && c.ContractNo == contract.ContractNo)
                    .Select(c => new CashFlow { ContractNo = c.ContractNo,
                        ValueDate = c.ValueDate.ToUniversalTime(),
                        Month = c.Month,
                        Benefit = c.Benefit })
                    .ToListAsync();
                _context.RemoveRange(cashflowsToDelete);
                _context.AddRange(cashflows);
                TempData["Message"] = "Cashflows Updated";
            }
            else
            {
                _context.AddRange(cashflows);
                TempData["Message"] = "Cashflows Inserted";
            }

            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index), new { selectedContractNo = contract.ContractNo });
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

    private async Task<DateTime> GetValueDateAsync()
    {
        // Get the latest value date that exists in both the contracts and risk free rate tables
        var valueDate = await _context.Contracts.Select(c => c.ValueDate).Intersect(
            _context.RiskFreeRates.Select(r => r.ValueDate)).MaxAsync();
        return DateTime.SpecifyKind(valueDate, DateTimeKind.Utc);
    }

    private async Task<Contract> GetContractAsync(DateTime valueDate, int contractNo)
    {
        var contract = await _context.Contracts.Where(c => c.ValueDate == valueDate && c.ContractNo == contractNo)
            .FirstOrDefaultAsync();
        return contract;
    }

    private async Task<List<RiskFreeRateData>> GetDiscountAsync(DateTime valueDate)
    {
        var projId = await _context.RiskFreeRates.Where(r => r.ValueDate == valueDate).Select(r => r.ProjectionId)
            .FirstOrDefaultAsync();
        var riskFreeRateData = await _context.RiskFreeRateData.Where(r => r.ProjectionId == projId)
            .OrderBy(r => r.Month).ToListAsync();
        return riskFreeRateData;
    }

    private List<CashFlow> GetDiscountedCashFlows(List<CashFlow> cashFlows, List<RiskFreeRateData> riskFreeRateData)
    {
        var discountedCashFlows = new List<CashFlow>();
        foreach (CashFlow cf in cashFlows)
        {
            var discountedCashFlow = new CashFlow
            {
                ContractNo = cf.ContractNo,
                ValueDate = cf.ValueDate,
                Month = cf.Month,
                Benefit = cf.Benefit * riskFreeRateData.Where(r => r.Month == cf.Month).Select(r => r.Price)
                    .FirstOrDefault()
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