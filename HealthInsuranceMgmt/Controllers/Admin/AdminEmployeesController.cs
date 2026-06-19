using HealthInsuranceMgmt.Data;
using HealthInsuranceMgmt.Models.Entities;
using HealthInsuranceMgmt.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthInsuranceMgmt.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminEmployeesController : Controller
    {
        private readonly UserManager<EmpRegister> _userManager;
        private readonly ApplicationDbContext _context; // Database context add kiya

        public AdminEmployeesController(UserManager<EmpRegister> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // INDEX: Saari employees ki list
        public async Task<IActionResult> Index()
        {
            var employees = await _userManager.GetUsersInRoleAsync("Employee");
            return View(employees);
        }

        // CREATE: Form dikhana
        public IActionResult Create()
        {
            return View();
        }

        // CREATE: Form save karna
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminEmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new EmpRegister
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Department = model.Department,
                    Designation = model.Designation,
                    Address = model.Address,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Employee");
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        // EDIT: Form dikhana
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var model = new AdminEmployeeViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Department = user.Department,
                Designation = user.Designation,
                Address = user.Address
            };

            return View(model);
        }

        // EDIT: Form save karna
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, AdminEmployeeViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null) return NotFound();

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Department = model.Department;
                user.Designation = model.Designation;
                user.Address = model.Address;
                user.Email = model.Email;
                user.UserName = model.Email;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        // DELETE: Confirmation page
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            return View(user);
        }

        // DELETE: Actual delete (With Safety Check)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            // SAFETY CHECK: Check karo ki employee ne koi policy request toh nahi bheji?
            bool hasRequests = _context.PolicyRequestDetails.Any(r => r.EmpRegisterId == id);
            // Check karo ki employee ko koi policy assign toh nahi hui?
            bool hasAssignedPolicies = _context.PoliciesOnEmployees.Any(p => p.EmpRegisterId == id);

            if (hasRequests || hasAssignedPolicies)
            {
                // Agar data hai, toh delete mat karo, error message do
                TempData["Error"] = "Cannot delete this employee because they have existing policy requests or assigned policies. Please reject their requests or remove assigned policies first.";
                return RedirectToAction(nameof(Delete), new { id = id });
            }

            // Agar koi data nahi hai, toh safely delete karo
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["Message"] = "Employee deleted successfully.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(user);
        }
    }
}