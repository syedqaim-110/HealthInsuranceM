// Controllers/Employee/EmployeeDashboardController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthInsuranceMgmt.Controllers.Employee
{
    // Sirf "Employee" role wale log isko access kar sakte hain
    [Authorize(Roles = "Employee")]
    public class EmployeeDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}