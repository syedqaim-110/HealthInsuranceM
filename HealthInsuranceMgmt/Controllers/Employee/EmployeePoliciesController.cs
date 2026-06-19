// Controllers/Employee/EmployeePoliciesController.cs
using HealthInsuranceMgmt.Data;
using HealthInsuranceMgmt.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthInsuranceMgmt.Controllers.Employee
{
    [Authorize(Roles = "Employee")]
    public class EmployeePoliciesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<EmpRegister> _userManager;

        public EmployeePoliciesController(ApplicationDbContext context, UserManager<EmpRegister> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // SEARCH: Saari available policies dikhana
        public async Task<IActionResult> Index(string searchString)
        {
            // Database se saari policies aur unki companies nikal lo
            var policies = from p in _context.Policies.Include(p => p.Company)
                           select p;

            // Agar user ne search box mein kuch type kiya hai
            if (!string.IsNullOrEmpty(searchString))
            {
                policies = policies.Where(p => p.PolicyName.Contains(searchString) || p.PolicyType.Contains(searchString));
            }

            return View(await policies.ToListAsync());
        }

        // REQUEST: Employee policy request karega
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestPolicy(int policyId)
        {
            // Kaunsa employee request kar raha hai? (Current logged in user)
            var user = await _userManager.GetUserAsync(User);

            // Check karo ki employee ne pehle se toh nahi maanga hai
            var existingRequest = await _context.PolicyRequestDetails
                .FirstOrDefaultAsync(r => r.EmpRegisterId == user.Id && r.PolicyId == policyId && r.Status == "Pending");

            if (existingRequest != null)
            {
                TempData["Message"] = "Aapne is policy ke liye pehle hi request bhej di hai. Kripya Admin ke response ka wait karein.";
                return RedirectToAction(nameof(Index));
            }

            // Naya request object banao
            var request = new PolicyRequestDetail
            {
                EmpRegisterId = user.Id,
                PolicyId = policyId,
                RequestDate = DateTime.Now,
                Status = "Pending"
            };

            _context.Add(request);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Aapki request successfully bhej di gayi hai!";
            return RedirectToAction(nameof(Index));
        }
    }
}