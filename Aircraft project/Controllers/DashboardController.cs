using Microsoft.AspNetCore.Mvc;

namespace Aircraft_project.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View("dashboard"); 
        }

        public IActionResult Aircraft()
        {
            return View("Aircraft");
        }

        public IActionResult AddAircraft()
        {
            return View("AddAircraft");
        }
    }
}
