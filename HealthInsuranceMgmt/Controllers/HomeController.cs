using HealthInsuranceMgmt.Models;
using HealthInsuranceMgmt.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HealthInsuranceMgmt.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<EmpRegister> _userManager;

        // UserManager inject kiya taaki hum logged in user ki details nikal saken
        public HomeController(ILogger<HomeController> logger, UserManager<EmpRegister> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        // Yeh main function hai jo redirect karega
        public async Task<IActionResult> Index()
        {
            // Agar user login hua hai
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    // Check karo user ka kya role hai
                    if (await _userManager.IsInRoleAsync(user, "Admin"))
                    {
                        return RedirectToAction("Index", "AdminDashboard");
                    }
                    else if (await _userManager.IsInRoleAsync(user, "Employee"))
                    {
                        return RedirectToAction("Index", "EmployeeDashboard");
                    }
                    else if (await _userManager.IsInRoleAsync(user, "FinanceManager"))
                    {
                        return RedirectToAction("Index", "FinanceDashboard");
                    }
                }
            }

            // Agar login nahi hai, toh normal Home page dikhao
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Feedback()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}