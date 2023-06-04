using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ActuarialApplications.Models;

namespace ActuarialApplications.Controllers
{
    public class ContractController : Controller
    {
        private readonly LocalLifeDbContext _context;

        public ContractController(LocalLifeDbContext context)
        {
            _context = context;
        }

        // GET: Contracts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Contracts.ToListAsync());
        }

        // GET: Contracts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contracts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ValueDate,ContractNo,ValueDate,BirthDate,Sex,VestingAge,GuaranteeBenefit,PayPeriod,Table")] Contract contract)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contract);
        }

        // GET: Contracts/Edit/5
       
        // GET: Contracts/Delete/5
        public async Task<IActionResult> Delete(DateTime ValueDate, int ContractNo)
        {
            if (ValueDate == null)
            {
                return NotFound();
            }

            var contract = await _context.Contracts
                .FirstOrDefaultAsync(m => m.ValueDate == ValueDate && m.ContractNo == ContractNo);
            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // POST: Contracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(DateTime ValueDate, int ContractNo)
        {
            var contract = await _context.Contracts.FindAsync(ValueDate, ContractNo);
            _context.Contracts.Remove(contract);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContractExists(DateTime ValueDate, int ContractNo)
        {
            return _context.Contracts.Any(e => e.ValueDate == ValueDate && e.ContractNo == ContractNo);
        }
    }
}
