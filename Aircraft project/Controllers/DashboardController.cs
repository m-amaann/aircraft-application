using Aircraft_project.Data;
using Aircraft_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Linq;




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


        // Admin Create Function of POST Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAdmin(Admin admin)
        {
            if (ModelState.IsValid)
            {
                admin.Password = HashPassword(admin.Password); // Hash the password
                _context.Admins.Add(admin);
                _context.SaveChanges(); 
                return RedirectToAction("Admin"); // Redirect to the admin page view
            }

            return View(admin);
        }




        //HASH Password Function

        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }




        // Admin Login Function
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminLogin(Admin admin)
        {
            var adminInDb = _context.Admins.FirstOrDefault(a => a.Email == admin.Email);
            if (adminInDb != null)
            {
                string hashedPassword = HashPassword(admin.Password);
                if (hashedPassword == adminInDb.Password)
                {
                    // Authentication successful
                    // Set session or authentication cookie here
                    return RedirectToAction("dashboard"); // Redirect to dashboard
                }
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View("AdminLogin", admin);
        }






        // IN BELOW WE HAVE PROVIDED CODES FOR  THE (VIEWS) 

        //Admin Login 
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

        // Admin View
 public IActionResult Admin()
    {
        var admins = _context.Admins.ToList();
        return View(admins);
    }


        public ActionResult CreateAdmin()
        {
            return View();
        }
    }
}
