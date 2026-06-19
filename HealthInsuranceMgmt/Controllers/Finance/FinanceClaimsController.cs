// Controllers/Finance/FinanceClaimsController.cs
using HealthInsuranceMgmt.Data;
using HealthInsuranceMgmt.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthInsuranceMgmt.Controllers.Finance
{
    [Authorize(Roles = "FinanceManager")]
    public class FinanceClaimsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FinanceClaimsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // INDEX: Pending Payments (Jo policies assign hui hain par paisa abhi nahi diya)
        public async Task<IActionResult> Index()
        {
            var pendingClaims = await _context.PoliciesOnEmployees
                .Include(p => p.Employee)
                .Include(p => p.Policy)
                .Where(p => p.Status == "Active" && p.ClaimAmountCredited == null)
                .OrderByDescending(p => p.AssignedDate)
                .ToListAsync();

            return View(pendingClaims);
        }

        // SETTLE: Payment credit karne ka form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SettlePayment(int assignId, decimal amountCredited)
        {
            // 1. Record dhoondo
            var assignedPolicy = await _context.PoliciesOnEmployees
                .Include(p => p.Policy)
                .FirstOrDefaultAsync(p => p.Id == assignId);

            if (assignedPolicy == null) return NotFound();

            // 2. Amount aur Status update karo
            assignedPolicy.ClaimAmountCredited = amountCredited;
            assignedPolicy.ClaimSettledDate = DateTime.Now;
            assignedPolicy.Status = "Settled"; // Request close ho gayi

            _context.Update(assignedPolicy);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Payment successfully credited and claim has been settled.";
            return RedirectToAction(nameof(Index));
        }

        // SETTLED CLAIMS: Jinka payment ho chuka hai
        public async Task<IActionResult> SettledClaims()
        {
            var settledClaims = await _context.PoliciesOnEmployees
                .Include(p => p.Employee)
                .Include(p => p.Policy)
                .Where(p => p.Status == "Settled")
                .OrderByDescending(p => p.ClaimSettledDate)
                .ToListAsync();

            return View(settledClaims);
        }
    }
}