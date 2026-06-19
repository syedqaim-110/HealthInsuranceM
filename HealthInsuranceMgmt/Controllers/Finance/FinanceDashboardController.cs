// Controllers/Finance/FinanceDashboardController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthInsuranceMgmt.Controllers.Finance
{
    [Authorize(Roles = "FinanceManager")]
    public class FinanceDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}