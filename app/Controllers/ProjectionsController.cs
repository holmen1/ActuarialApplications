using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ActuarialApplications.Models;
using System.Text.Json;

namespace ActuarialApplications.Controllers
{
    public class ProjectionsController : Controller
    {
        private readonly LocalDbContext _context;
        private readonly string _ibnrUri;

        // HttpClient lifecycle management best practices:
        // https://learn.microsoft.com/dotnet/fundamentals/networking/http/httpclient-guidelines#recommended-use
        private static HttpClient _sharedClient;

        public ProjectionsController(LocalDbContext context, IConfiguration config)
        {
            _context = context;
            _ibnrUri = config.GetValue<string>("Serverless:rfr");
            _sharedClient = new()
            {
                BaseAddress = new Uri(_ibnrUri),
            };
        }

        // GET: Projections
        // where ValueDate is the most recent date in the database
        public async Task<IActionResult> Index()
        {
            var valueDate = await _context.RiskFreeRates.MaxAsync(r => r.ValueDate);
            var rfr = await _context.RiskFreeRates.Where(r => r.ValueDate == valueDate).ToListAsync();
            return View(rfr);
        }

        // GET: Projections/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projections/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ufr,ConvergenceMaturity,Tol")] double ufr, int convergencematurity, double tol)
        {

            var swParameters = new SwParameters
            {
                ParRates = new List<double> { 0.03495, 0.0324, 0.0298, 0.02855 },
                ParMaturities = new List<int> { 2, 3, 5, 10 },
                Projection = new List<int> { 1, 151 },
                // From input form
                Ufr = ufr,
                ConvergenceMaturity = convergencematurity,
                Tol = tol
            };

            //DateTime valueDate = DateTime.Parse("2023-03-31");
            DateTime valueDate = DateTime.Now;

            using HttpResponseMessage response = await _sharedClient.PostAsJsonAsync("rates/", swParameters);
            var resp = await response.Content.ReadFromJsonAsync<Response>();

            // List of RiskFreeRate objects where Maturity is the index + 1
            var rfr = resp.rfr.Select((value, index) => new RiskFreeRate { ValueDate = valueDate, Maturity = index + 1, Value = value }).ToList();

            if (ModelState.IsValid)
            {
                _context.AddRange(rfr);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
