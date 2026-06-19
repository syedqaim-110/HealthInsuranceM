// Controllers/Admin/AdminFAQsController.cs
using HealthInsuranceMgmt.Data;
using HealthInsuranceMgmt.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthInsuranceMgmt.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminFAQsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminFAQsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // INDEX: Sabhi questions dekho (Answered aur Unanswered dono)
        public async Task<IActionResult> Index()
        {
            var faqs = await _context.FAQs
                .Include(f => f.AskedBy)
                .OrderByDescending(f => f.AskedOn)
                .ToListAsync();

            return View(faqs);
        }

        // REPLY: Admin answer dega aur publish karega
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reply(int id, string answer)
        {
            var faq = await _context.FAQs.FindAsync(id);
            if (faq == null) return NotFound();

            if (!string.IsNullOrEmpty(answer))
            {
                faq.Answer = answer;
                faq.AnsweredOn = DateTime.Now;
                faq.IsPublished = true; // Reply dete hi public page par dikhane ke liye

                _context.Update(faq);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Reply successfully submitted and published.";
            }
            else
            {
                TempData["Error"] = "Answer cannot be empty.";
            }

            return RedirectToAction(nameof(Index));
        }

        // DELETE: Question delete karna
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var faq = await _context.FAQs.FindAsync(id);
            if (faq != null)
            {
                _context.FAQs.Remove(faq);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Question deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}