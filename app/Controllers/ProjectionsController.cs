using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ActuarialApplications.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ActuarialApplications.Controllers
{
    public class ProjectionsController : Controller
    {
        private readonly LocalRateDbContext _context;
        private readonly string _ibnrUri;

        // HttpClient lifecycle management best practices:
        // https://learn.microsoft.com/dotnet/fundamentals/networking/http/httpclient-guidelines#recommended-use
        private static HttpClient _sharedClient;

        private SelectList SwapValueDates;
        private List<int> SwapMaturities;
        private List<double> SwapRates;

        public ProjectionsController(LocalRateDbContext context, IConfiguration config)
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
        public async Task<IActionResult> Index([Bind("SelectedDate")] DateTime selectedDate)
        {
            SwapValueDates = new SelectList(await _context.Swap.Select(s => s.ValueDate).Distinct()
                .OrderByDescending(s => s).ToListAsync());
            if (selectedDate == DateTime.MinValue)
            {
                selectedDate = _context.Swap.Select(s => s.ValueDate).DefaultIfEmpty().Max();
            }

            var swapRates = await _context.Swap.Where(s => s.ValueDate == selectedDate).OrderBy(s => s.Tenor)
                .ToListAsync();

            int maxProjectionId = await _context.RiskFreeRates.Where(r => r.ValueDate == selectedDate)
                .Select(r => r.ProjectionId).DefaultIfEmpty().MaxAsync();
            var rfr = await _context.RiskFreeRateData.Where(r => r.ProjectionId == maxProjectionId).ToListAsync();
            var param = await _context.RiskFreeRates.FindAsync(maxProjectionId);

            return View(new ProjectionIndexModel
            {
                ValueDates = SwapValueDates,
                SelectedDate = selectedDate,
                SwapRates = swapRates,
                Rfr = rfr,
                Param = param
            });
        }


        // POST: Projections/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SelectedDate,Ufr,ConvergenceMaturity")] DateTime selecteddate,
            double ufr, int convergencematurity, double tol)
        {
            var swapRates = await _context.Swap.Where(s => s.ValueDate == selecteddate)
                .ToDictionaryAsync(s => s.Tenor, s => s.Value);
            // Ensure that keys are in ascending order
            swapRates = swapRates.OrderBy(s => s.Key).ToDictionary(s => s.Key, s => s.Value);
            SwapRates = swapRates.Values.ToList();
            SwapMaturities = swapRates.Keys.ToList();

            var swParameters = new FastApiRequestParameters
            {
                ParRates = SwapRates,
                ParMaturities = SwapMaturities,
                Projection = new List<int> { 1, 151 },
                Tol = 1E-4,
                // From input form
                Ufr = ufr,
                ConvergenceMaturity = convergencematurity
            };

            // Create string from swParameters
            var swParametersJson = JsonSerializer.Serialize(swParameters);
            DateTime createDate = DateTime.Now;
            // Create incremental integer index key for database
            int projectionId = _context.RiskFreeRates.Select(r => r.ProjectionId).DefaultIfEmpty().Max() + 1;

            using HttpResponseMessage response = await _sharedClient.PostAsJsonAsync("rates/", swParameters);
            var resp = await response.Content.ReadFromJsonAsync<FastApiResponse>();

            // List of RiskFreeRate objects where Maturity is the index + 1
            var rates = resp.rfr.Select((value, index) => new RiskFreeRateData
                { ProjectionId = projectionId, Maturity = index + 1, SpotValue = value, }).ToList();
            var parameters = new RiskFreeRate
            {
                ProjectionId = projectionId,
                ValueDate = selecteddate,
                LastUpdated = createDate,
                RequestParameters = swParametersJson
            };

            if (ModelState.IsValid)
            {
                _context.RiskFreeRateData.AddRange(rates);
                _context.RiskFreeRates.AddRange(parameters);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool RiskFreeRateExists(int id)
        {
            return _context.RiskFreeRates.Any(e => e.ProjectionId == id);
        }
    }
}