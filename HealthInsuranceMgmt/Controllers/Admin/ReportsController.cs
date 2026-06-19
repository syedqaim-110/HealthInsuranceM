// Controllers/Admin/ReportsController.cs
using HealthInsuranceMgmt.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthInsuranceMgmt.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(DateTime? startDate, DateTime? endDate)
        {
            // 1. Summary Calculations
            ViewBag.TotalCompanies = _context.CompanyDetails.Count();
            ViewBag.TotalPolicies = _context.Policies.Count();
            ViewBag.TotalEmployees = _context.Users.Count();

            // Total premium collected from settled claims
            ViewBag.TotalRevenue = _context.PoliciesOnEmployees
                .Where(p => p.Status == "Settled" && p.ClaimAmountCredited != null)
                .Sum(p => p.ClaimAmountCredited);

            // 2. Billing Report (Date Filter)
            // Agar date na diye jayein toh default last 30 days le lo
            if (!startDate.HasValue) startDate = DateTime.Now.AddDays(-30);
            if (!endDate.HasValue) endDate = DateTime.Now;

            ViewBag.StartDate = startDate.Value.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate.Value.ToString("yyyy-MM-dd");

            var billingReports = _context.PoliciesOnEmployees
                .Include(p => p.Employee)
                .Include(p => p.Policy)
                .Where(p => p.Status == "Settled" &&
                            p.ClaimSettledDate >= startDate &&
                            p.ClaimSettledDate <= endDate.Value.AddDays(1))
                .OrderByDescending(p => p.ClaimSettledDate)
                .ToList();

            return View(billingReports);
        }
    }
}