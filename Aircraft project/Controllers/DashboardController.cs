using Aircraft_project.Data;
using Aircraft_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using Microsoft.AspNetCore.Authorization;


namespace Aircraft_project.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Constructor
        public DashboardController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // Index action to display the dashboard or redirect to AdminLogin
        public IActionResult Index()
        {
            // Check if the user is authenticated (you can customize this check based on your authentication logic)
            if (!User.Identity.IsAuthenticated)
            {
                // Redirect to AdminLogin if not authenticated
                return RedirectToAction("AdminLogin");
            }

            // User is authenticated, display the dashboard
            return View("dashboard");
        }



        // Admin Create Function of POST Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAdmin(Admin admin)
        {
            if (ModelState.IsValid)
            {
                // Check if an admin with the same email already exists
                if (_context.Admins.Any(a => a.Email == admin.Email))
                {
                    ModelState.AddModelError("Email", "An admin with this email already exists.");
                    return View(admin);
                }

                admin.Password = HashPassword(admin.Password); // Hash the password
                _context.Admins.Add(admin);
                _context.SaveChanges();
                return RedirectToAction("Admin");
            }

            return View(admin);
        }


        // HASH Password Function
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

            if (adminInDb != null && IsPasswordValid(admin.Password, adminInDb.Password))
            {
                // Authentication successful
                // You may want to set a session or authentication cookie here
                HttpContext.Session.SetString("AdminEmail", adminInDb.Email);
                HttpContext.Session.SetString("AdminName", adminInDb.Name);

                return RedirectToAction("Indexes");
            }

            // Authentication failed
            ModelState.AddModelError("", "Invalid login attempt.");
            return View("AdminLogin", admin);
        }


        private bool IsPasswordValid(string enteredPassword, string storedHashedPassword)
        {
            // Compare the entered password with the stored hashed password
            return HashPassword(enteredPassword) == storedHashedPassword;
        }


        // IN BELOW WE HAVE PROVIDED CODES FOR  THE (VIEWS) 

        //Admin Login 
        public ActionResult AdminLogin()
        {
            return View();
        }

        public IActionResult Indexes()
        {
            return View("dashboard");
        }



        public IActionResult Aircraft()
        {
            // return View("Aircraft");
            var aircraft = _context.Aircraft.ToList();
            return View(aircraft);
        }

        public IActionResult AddAircraft()
        {
            return View("AddAircraft");
        }

        [HttpPost]
        public IActionResult CreateAircraft(Aircraft aircraft)
        {
            if (ModelState.IsValid)
            {
                // if (imageFile != null && imageFile.Length > 0)
                // {
                //     aircraft.ImagePath = UploadImage(imageFile);
                // }

                _context.Aircraft.Add(aircraft);
                _context.SaveChanges();
                return RedirectToAction("AddAircraft");
            }

            return View("AddAircraft", aircraft);
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
