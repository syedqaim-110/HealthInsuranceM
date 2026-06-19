using HealthInsuranceMgmt.Data;
using HealthInsuranceMgmt.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HealthInsuranceMgmt.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class PoliciesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PoliciesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var policies = await _context.Policies.Include(p => p.Company).ToListAsync();
            return View(policies);
        }

        public IActionResult Create()
        {
            if (!_context.CompanyDetails.Any())
            {
                TempData["Message"] = "Please add an Insurance Company first.";
                return RedirectToAction("Create", "CompanyDetails");
            }
            PopulateCompanyDropdown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Policy policy)
        {
            ModelState.Remove("Company");

            if (ModelState.IsValid)
            {
                _context.Add(policy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateCompanyDropdown();
            return View(policy);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var policy = await _context.Policies.FindAsync(id);
            if (policy == null) return NotFound();

            PopulateCompanyDropdown(policy.CompanyDetailId);
            return View(policy);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Policy policy)
        {
            if (id != policy.Id) return NotFound();

            ModelState.Remove("Company");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(policy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PolicyExists(policy.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            PopulateCompanyDropdown(policy.CompanyDetailId);
            return View(policy);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var policy = await _context.Policies.Include(p => p.Company).FirstOrDefaultAsync(m => m.Id == id);
            if (policy == null) return NotFound();
            return View(policy);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var policy = await _context.Policies.FindAsync(id);
            if (policy != null)
            {
                _context.Policies.Remove(policy);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool PolicyExists(int id)
        {
            return _context.Policies.Any(e => e.Id == id);
        }

        private void PopulateCompanyDropdown(object? selectedCompany = null)
        {
            var companies = _context.CompanyDetails.OrderBy(c => c.CompanyName).ToList();
            ViewBag.CompanyList = new SelectList(companies, "Id", "CompanyName", selectedCompany);
        }
    }
}