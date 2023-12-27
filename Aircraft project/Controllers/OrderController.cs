using Microsoft.AspNetCore.Mvc;

namespace Aircraft_project.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
