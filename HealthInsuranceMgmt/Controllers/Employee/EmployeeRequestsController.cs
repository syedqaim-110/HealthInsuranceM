using HealthInsuranceMgmt.Data;
using HealthInsuranceMgmt.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthInsuranceMgmt.Controllers.Employee
{
    [Authorize(Roles = "Employee")]
    public class EmployeeRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<EmpRegister> _userManager;

        public EmployeeRequestsController(ApplicationDbContext context, UserManager<EmpRegister> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // MY REQUESTS: Employee ne jo jo requests bheji hain
        public async Task<IActionResult> MyRequests()
        {
            var user = await _userManager.GetUserAsync(User);

            // Current user ki saari requests nikal lo, Policy aur Approval details ke sath
            var requests = await _context.PolicyRequestDetails
                .Include(r => r.Policy) // Request kis policy ke liye thi
                    .ThenInclude(p => p.Company) // Policy kis company ki thi
                .Include(r => r.ApprovalDetail) // Admin ne approve kiya ya nahi
                .Where(r => r.EmpRegisterId == user.Id)
                .OrderByDescending(r => r.RequestDate) // Sabse naya upar
                .ToListAsync();

            return View(requests);
        }

        // MY POLICIES: Jo policies employee ko assign hui hain
        public async Task<IActionResult> MyPolicies()
        {
            var user = await _userManager.GetUserAsync(User);

            // Database se un policies ki list nikalo jo is user ko assign hain
            var assignedPolicies = await _context.PoliciesOnEmployees
                .Include(p => p.Policy) // Policy details
                    .ThenInclude(c => c.Company) // Company details
                .Where(p => p.EmpRegisterId == user.Id && p.Status == "Active")
                .ToListAsync();

            return View(assignedPolicies);
        }
        // TIMELINE: Request ki detailed timeline dekhna
        public async Task<IActionResult> Timeline(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            // Request ki details nikalo
            var request = await _context.PolicyRequestDetails
                .Include(r => r.Policy)
                .Include(r => r.ApprovalDetail)
                .FirstOrDefaultAsync(r => r.Id == id && r.EmpRegisterId == user.Id);

            if (request == null) return NotFound();

            // Assigned Policy nikalo (agar assign hui hai)
            var assignedPolicy = await _context.PoliciesOnEmployees
                .FirstOrDefaultAsync(p => p.EmpRegisterId == user.Id && p.PolicyId == request.PolicyId);

            ViewBag.AssignedPolicy = assignedPolicy;
            return View(request);
        }
    }
}
