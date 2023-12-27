using Microsoft.AspNetCore.Mvc;

namespace Aircraft_project.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
