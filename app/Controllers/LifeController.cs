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

    //private readonly LocalLifeDbContext _context;
    private readonly AirflowDbContext _context;

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

        var indexModel = new LifeIndexModel
        {
            contractNoList = contractNoList,
            SelectedContractNo = selectedContractNo,
            contract = await _context.Contracts.FindAsync(selectedContractNo),
            cashFlows = await _context.CashFlows.Where(c => c.ContractNo == selectedContractNo).ToListAsync()
        };

        return View(indexModel);
    }

    // GET: Life/Create [Cashflows]
    public async Task<IActionResult> Create(
        [Bind("ValueDate,ContractNo,ValueDate,BirthDate,Sex,VestingAge,GuaranteeBenefit,PayPeriod,Table")]
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
}