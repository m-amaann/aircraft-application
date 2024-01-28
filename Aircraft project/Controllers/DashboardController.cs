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

        public IActionResult Index()
        {
            // Check if the user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                // Redirect to AdminLogin 
                return RedirectToAction("AdminLogin");
            }

            // Fetch counts for various entities
            var usersCount = _context.Users.Count();
            var aircraftCount = _context.Aircraft.Count();
            var adminsCount = _context.Admins.Count();
            var ordersCount = _context.Orders.Count();

            // Add these counts to ViewData
            ViewData["TotalUsersCount"] = usersCount;
            ViewData["TotalAircraftCount"] = aircraftCount;
            ViewData["TotalAdminsCount"] = adminsCount;
            ViewData["TotalOrdersCount"] = ordersCount;

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

                TempData["SuccessMessage"] = "You are logged in successfully!";

                // Directly set user ID and username in local storage using ViewData
                ViewData["AdminId"] = adminInDb.AdminId.ToString();
                ViewData["AdminName"] = adminInDb.Name;

                // return RedirectToAction("Indexes");
                return View();
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


        public IActionResult Inventory()
        {
            var inventory = _context.Inventory.ToList();
            return View(inventory);
        }

        public IActionResult Tracking()
        {
            // return View("Aircraft");
            var tracking = _context.Trackings.ToList();
            return View(tracking);
        }

        [HttpPost]
        public IActionResult UpdateTrackingStatus(int trackingId, string status)
        {
            // _logger.LogInformation($"Attempting to update tracking ID {trackingId} with status {status}.");
            var tracking = _context.Trackings.FirstOrDefault(t => t.Id == trackingId);
            if (tracking != null)
            {
                tracking.Status = status;

                _context.SaveChanges();

                return Json(new { success = true, message = "Tracking status updated successfully." });
            }
            else
            {
                // _logger.LogWarning($"No tracking record found for ID {trackingId}.");
                return Json(new { success = false, message = "Tracking record not found." });
            }
        }

        [HttpPost]
        public IActionResult AddToInventory(Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                // Additional logic, if needed, before adding to the inventory

                _context.Inventory.Add(inventory);
                _context.SaveChanges();

                return RedirectToAction("Inventory");
            }

            // If the model state is not valid, return to the view with the current inventory
            return View("Inventory", _context.Inventory.ToList());
        }

        [HttpGet]
        public IActionResult GetAircraftDetails(int id)
        {
            // Fetch the aircraft record by ID
            var aircraft = _context.Aircraft.Find(id);
            return Json(aircraft);
        }

        [HttpPost]
        public IActionResult UpdateAircraft([FromBody] Aircraft model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest("Invalid data. The provided model is null.");
                }

                // Ensure the aircraft with the given ID exists
                var existingAircraft = _context.Aircraft.Find(model.Id);
                if (existingAircraft == null)
                {
                    return NotFound($"Aircraft with ID {model.Id} not found.");
                }

                // Update the properties of the existing aircraft with the new data
                existingAircraft.Name = model.Name;
                existingAircraft.Manufacturer = model.Manufacturer;
                existingAircraft.Color = model.Color;
                existingAircraft.PassengerCapacity = model.PassengerCapacity;
                existingAircraft.MaxSpeed = model.MaxSpeed;
                existingAircraft.FuelCapacity = model.FuelCapacity;
                existingAircraft.EngineType = model.EngineType;
                existingAircraft.Range = model.Range;
                // existingAircraft.ManufacturingDate = model.ManufacturingDate;
                existingAircraft.AdditionalInformation = model.AdditionalInformation;
                existingAircraft.ImagePath = model.ImagePath;
                existingAircraft.Category = model.Category;
                // existingAircraft.Price = model.Price;

                _context.SaveChanges();

                return Ok(); // You can return a success status or other data if needed
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Exception in UpdateAircraft: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        public IActionResult DeleteAircraft(int id)
        {
            var aircraft = _context.Aircraft.Find(id);

            if (aircraft != null)
            {
                _context.Aircraft.Remove(aircraft);
                _context.SaveChanges();

                return Json(new { success = true, message = "Aircraft deleted successfully." });
            }
            else
            {
                return Json(new { success = false, message = "Aircraft not found." });
            }
        }

    }
}
