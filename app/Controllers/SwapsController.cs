using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ActuarialApplications.Models;

namespace ActuarialApplications.Controllers
{
    public class SwapsController : Controller
    {
        private readonly LocalDbContext _context;

        public SwapsController(LocalDbContext context)
        {
            _context = context;
        }

        // GET: Swaps
        public async Task<IActionResult> Index()
        {
            return View(await _context.Swap.ToListAsync());
        }

        // GET: Swaps/Details/5
        public async Task<IActionResult> Details(DateTime ValueDate, string Currency, int Tenor)
        {
        if (ValueDate == null || Currency == null)
            {
                return NotFound();
            }

            var swap = await _context.Swap
                .FirstOrDefaultAsync(m => m.ValueDate == ValueDate && m.Currency == Currency && m.Tenor == Tenor);
            if (swap == null)
            {
                return NotFound();
            }

            return View(swap);
        }

        // GET: Swaps/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Swaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ValueDate,Currency,Tenor,SettlementFreq,Value")] Swap swap)
        {
            if (ModelState.IsValid)
            {
                _context.Add(swap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(swap);
        }

        // GET: Swaps/Edit/5
        public async Task<IActionResult> Edit(DateTime ValueDate, string Currency, int Tenor)
        {
            if (Currency == null)
            {
                return NotFound();
            }

            var swap = await _context.Swap.FindAsync(ValueDate, Currency, Tenor);
            if (swap == null)
            {
                return NotFound();
            }
            return View(swap);
        }

        // POST: Swaps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DateTime ValueDate, string Currency, int Tenor, [Bind("Id,ValueDate,Currency,Tenor,SettlementFreq,Value")] Swap swap)
        {
            if (ValueDate != swap.ValueDate)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(swap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SwapExists(swap.ValueDate, swap.Currency, swap.Tenor))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(swap);
        }

        // GET: Swaps/Delete/5
        public async Task<IActionResult> Delete(DateTime ValueDate, string Currency, int Tenor)
        {
            if (ValueDate == null)
            {
                return NotFound();
            }

            var swap = await _context.Swap
                .FirstOrDefaultAsync(m => m.ValueDate == ValueDate && m.Currency == Currency && m.Tenor == Tenor);
            if (swap == null)
            {
                return NotFound();
            }

            return View(swap);
        }

        // POST: Swaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(DateTime ValueDate, string Currency, int Tenor)
        {
            var swap = await _context.Swap.FindAsync(ValueDate, Currency, Tenor);
            _context.Swap.Remove(swap);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SwapExists(DateTime ValueDate, string Currency, int Tenor)
        {
            return _context.Swap.Any(e => e.ValueDate == ValueDate && e.Currency == Currency && e.Tenor == Tenor);
        }
    }
}
