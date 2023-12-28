using Aircraft_project.Data;
using Aircraft_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace Aircraft_project.Controllers
{
    public class ClientController : Controller
    {
		private readonly ApplicationDbContext _context;

		//Constructor
		public ClientController(ApplicationDbContext context)
		{
			_context = context;
		}


		[HttpPost]
        public IActionResult Register(Users user, string ConfirmPassword)
        {
            if (ModelState.IsValid)
            {
                if (user.password == ConfirmPassword)
                {
                    user.password = HashPassword(user.password);
                    _context.Users.Add(user);
                    _context.SaveChanges();
                    return RedirectToAction("Login"); // Redirect to login page after registration
                }
                ModelState.AddModelError("ConfirmPassword", "Passwords do not match");
            }
            return View(user);
        }


        private string HashPassword(string password)
		{
			using (var sha256 = SHA256.Create())
			{
				var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
				return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
			}
		}













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
        public IActionResult Shopping()
        {
            return View("Shopping");
        }
        [HttpGet]
        public IActionResult Contact()
        {
            return View("Contact");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }
    }
}
