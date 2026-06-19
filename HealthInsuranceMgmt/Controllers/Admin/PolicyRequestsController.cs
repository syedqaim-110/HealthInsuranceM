// Controllers/Admin/PolicyRequestsController.cs
using HealthInsuranceMgmt.Data;
using HealthInsuranceMgmt.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthInsuranceMgmt.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class PolicyRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<EmpRegister> _userManager;

        public PolicyRequestsController(ApplicationDbContext context, UserManager<EmpRegister> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // INDEX: Saari requests dekho
        public async Task<IActionResult> Index()
        {
            // Employee, Policy aur Approval details sab nikal lo
            var requests = await _context.PolicyRequestDetails
                .Include(r => r.Employee)
                .Include(r => r.Policy)
                .Include(r => r.ApprovalDetail)
                .OrderByDescending(r => r.RequestDate)
                .ToListAsync();

            return View(requests);
        }

        // APPROVE: Request ko approve karne ka action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int requestId, string remarks)
        {
            // 1. Request dhoondo
            var request = await _context.PolicyRequestDetails
                .Include(r => r.Policy)
                .FirstOrDefaultAsync(r => r.Id == requestId);

            if (request == null) return NotFound();

            // 2. Kaunsa Admin approve kar raha hai?
            var adminUser = await _userManager.GetUserAsync(User);

            // 3. Approval Detail entry banao
            var approval = new PolicyApprovalDetail
            {
                PolicyRequestDetailId = request.Id,
                ApprovedById = adminUser.Id,
                ApprovalDate = DateTime.Now,
                Status = "Approved",
                Remarks = string.IsNullOrEmpty(remarks) ? "Approved by Admin" : remarks
            };
            _context.Add(approval);

            // 4. Original request ka status update karo
            request.Status = "Approved";

            // 5. NAYA STEP: Policy Employee ko assign karo (PolicyOnEmployee table)
            var assignPolicy = new PolicyOnEmployee
            {
                EmpRegisterId = request.EmpRegisterId,
                PolicyId = request.PolicyId,
                AssignedDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddYears(1), // 1 saal ki validity
                Status = "Active"
            };
            _context.Add(assignPolicy);

            // 6. Database mein save karo
            await _context.SaveChangesAsync();

            TempData["Message"] = "Policy successfully approved and assigned to employee.";
            return RedirectToAction(nameof(Index));
        }

        // REJECT: Request ko reject karne ka action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int requestId, string remarks)
        {
            var request = await _context.PolicyRequestDetails.FindAsync(requestId);
            if (request == null) return NotFound();

            var adminUser = await _userManager.GetUserAsync(User);

            var approval = new PolicyApprovalDetail
            {
                PolicyRequestDetailId = request.Id,
                ApprovedById = adminUser.Id,
                ApprovalDate = DateTime.Now,
                Status = "Rejected",
                Remarks = string.IsNullOrEmpty(remarks) ? "Rejected by Admin" : remarks
            };
            _context.Add(approval);

            request.Status = "Rejected";

            await _context.SaveChangesAsync();

            TempData["Message"] = "Policy request has been rejected.";
            return RedirectToAction(nameof(Index));
        }
    }
}