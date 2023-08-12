using ActuarialApplications.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ActuarialApplications.Controllers;

public class ProjectionsController : Controller
{
    private readonly AirflowDbContext _context;

    private SelectList? SwapValueDates;

    public ProjectionsController(AirflowDbContext context, IConfiguration config)
    {
        _context = context;
    }

    // GET: Projections
    // where ValueDate is the most recent date in the database
    public async Task<IActionResult> Index([Bind("SelectedDate")] DateTime selectedDate)
    {
        SwapValueDates = new SelectList(await _context.Swap.Select(s => s.ValueDate.Date).Distinct()
            .OrderByDescending(s => s).ToListAsync());
        if (selectedDate == DateTime.MinValue)
            selectedDate = _context.Swap.Select(s => s.ValueDate).DefaultIfEmpty().Max();
        else // Necessary for postgresql to recognize the date as UTC
            selectedDate = DateTime.SpecifyKind(selectedDate, DateTimeKind.Utc);

        var swapRates = await _context.Swap.Where(s => s.ValueDate == selectedDate).OrderBy(s => s.Tenor)
            .ToListAsync();

        var maxProjectionId = await _context.RiskFreeRates.Where(r => r.ValueDate == selectedDate)
            .Select(r => r.ProjectionId).DefaultIfEmpty().MaxAsync();
        var rfr = await _context.RiskFreeRateData.
            Where(r => r.ProjectionId == maxProjectionId).
            OrderBy(r => r.Month).ToListAsync();
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
}