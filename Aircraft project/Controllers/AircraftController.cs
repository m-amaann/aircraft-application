using Microsoft.AspNetCore.Mvc;

namespace Aircraft_project.Controllers
{
    public class AircraftController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
