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
        public async Task<IActionResult> Details(DateTime? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var swap = await _context.Swap
                .FirstOrDefaultAsync(m => m.ValueDate == id);
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
        public async Task<IActionResult> Edit(DateTime? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var swap = await _context.Swap.FindAsync(id);
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
        public async Task<IActionResult> Edit(DateTime id, [Bind("Id,ValueDate,Currency,Tenor,SettlementFreq,Value")] Swap swap)
        {
            if (id != swap.ValueDate)
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
                    if (!SwapExists(swap.ValueDate))
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
        public async Task<IActionResult> Delete(DateTime? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var swap = await _context.Swap
                .FirstOrDefaultAsync(m => m.ValueDate == id);
            if (swap == null)
            {
                return NotFound();
            }

            return View(swap);
        }

        // POST: Swaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(DateTime id)
        {
            var swap = await _context.Swap.FindAsync(id);
            _context.Swap.Remove(swap);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SwapExists(DateTime id)
        {
            return _context.Swap.Any(e => e.ValueDate == id);
        }
    }
}
