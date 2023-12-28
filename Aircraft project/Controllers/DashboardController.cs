using Aircraft_project.Data;
using Aircraft_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Aircraft_project.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        //Constructor
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }



        // Admin Login POST Action Function
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Admin admin)
        {
            var adminInDb = _context.Admins.FirstOrDefault(a => a.Email == admin.Email);

            if (adminInDb != null && VerifyHashedPassword(adminInDb.Password, admin.Password))
            {
                // Logic for successful login
                // Set session or authentication cookie here
                return RedirectToAction("Dashboard");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View(admin);
        }




        // Admin Creation Function
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAdmin(Admin admin)
        {
            admin.Password = HashPassword(admin.Password);
            _context.Admins.Add(admin);
            _context.SaveChanges();
            return RedirectToAction("AdminList"); // Redirect to a list of admins
        }


        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            return HashPassword(password) == hashedPassword;
        }





        //Admin Login View
        public ActionResult AdminLogin()
        {
            return View();
        }

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

        // Admin Creation View
        public ActionResult Admin()
        {
            return View();
        }
        public ActionResult CreateAdmin()
        {
            return View();
        }
    }
}
