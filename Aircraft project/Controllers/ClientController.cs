using Microsoft.AspNetCore.Mvc;

namespace Aircraft_project.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            return View("home");
        }

        public IActionResult About()
        {
            return View("About");
        }

        public IActionResult Privacy()
        {
            return View("Privacy");
        }

        public IActionResult Services()
        {
            return View("ServicesPage");
        }

        public IActionResult Register()
        {
            return View("Register");
        }
    }
}
