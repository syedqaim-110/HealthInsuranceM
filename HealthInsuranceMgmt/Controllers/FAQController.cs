// Controllers/FAQController.cs
using HealthInsuranceMgmt.Data;
using HealthInsuranceMgmt.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthInsuranceMgmt.Controllers
{
    public class FAQController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<EmpRegister> _userManager;

        public FAQController(ApplicationDbContext context, UserManager<EmpRegister> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // INDEX: Public FAQ page (Jo questions publish hain)
        public async Task<IActionResult> Index()
        {
            // Sirf wahi questions dikhao jinka answer ho aur publish ho
            var faqs = await _context.FAQs
                .Where(f => f.IsPublished == true)
                .OrderByDescending(f => f.AnsweredOn)
                .ToListAsync();

            return View(faqs);
        }

        // ASK: User question puchne ke liye form bhejega
        [Authorize] // Sirf login wale user hi sawal puch sakte hain
        public IActionResult Ask()
        {
            return View();
        }

        // ASK (POST): Form submit hone par database mein save karega
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ask(FAQ model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                model.AskedById = user.Id;
                model.AskedOn = DateTime.Now;
                model.IsPublished = false; // Default false, jab admin reply de tab true hoga

                _context.Add(model);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Aapka question successfully submit ho gaya hai. Admin jaldi reply karega.";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}