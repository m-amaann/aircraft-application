using Microsoft.AspNetCore.Mvc;

namespace Aircraft_project.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View("dashboard"); 
        }
    }
}
