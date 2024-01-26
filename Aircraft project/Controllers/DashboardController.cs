﻿using Aircraft_project.Data;
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

        private readonly ILogger<DashboardController> _logger;

        // Constructor
        public DashboardController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            // Check if the user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                // Redirect to AdminLogin 
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

                admin.Password = HashPassword(admin.Password);
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

        public IActionResult Tracking()
        {
            // return View("Aircraft");
            var tracking = _context.Trackings.ToList();
            return View(tracking);
        }

        public IActionResult AddAircraft()
        {
            return View("AddAircraft");
        }

        

        [HttpPost]
        public IActionResult UpdateTrackingStatus(int trackingId, string status)
        {
            _logger.LogInformation($"Attempting to update tracking ID {trackingId} with status {status}.");
            var tracking = _context.Trackings.FirstOrDefault(t => t.Id == trackingId);
            if (tracking != null)
            {
                tracking.Status = status;

                _context.SaveChanges();

                return Json(new { success = true, message = "Tracking status updated successfully." });
            }
            else
            {
                _logger.LogWarning($"No tracking record found for ID {trackingId}.");
                return Json(new { success = false, message = "Tracking record not found." });
            }
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

        public IActionResult Users()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        public IActionResult Orders()
        {
            try
            {
                // Fetch all orders from the database
                var orders = _context.Orders.ToList();

                // Pass the orders to the view
                return View(orders);
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                Console.Error.WriteLine($"Exception in OrdersController: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        public ActionResult Reports()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetReportData(string modelType, DateTime fromDate, DateTime toDate)
        {
            // Fetch data based on the selected model and date range
            var entityType = _context.Model.FindEntityType(modelType);

            // Use reflection to dynamically invoke the Set method
            var setMethod = typeof(DbContext).GetMethod("Set").MakeGenericMethod(entityType.ClrType);
            var reportData = setMethod.Invoke(_context, null) as IQueryable<object>;

            // You may need to project the data based on the required columns
            var filteredData = reportData
                .Where(e => (DateTime)e.GetType().GetProperty("CreatedAt").GetValue(e) >= fromDate &&
                            (DateTime)e.GetType().GetProperty("CreatedAt").GetValue(e) <= toDate)
                .ToList();

            return Json(filteredData);
        }
    }
}
