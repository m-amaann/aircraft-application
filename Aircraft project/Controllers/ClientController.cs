using Aircraft_project.Data;
using System;
using System.IO;
using System.Threading.Tasks;
using Aircraft_project.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        // Register Method
        [HttpPost]
        public async Task<IActionResult> Register(Users user, string ConfirmPassword)
        {
            if (ModelState.IsValid)
            {
                if (user.Password == ConfirmPassword)
                {
                    try
                    {
                       
                        user.Password = HashPassword(user.Password); //Hashing

                        // Add the user to the context and save changes
                        _context.Users.Add(user);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Login"); // Redirect to login page after registration
                    }
                    catch (Exception ex)
                    {
                        
                        ModelState.AddModelError("", $"An unexpected error occurred: {ex.Message}");
                    }
                }
                else
                {
                    ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
                }
            }
   
            return View(user);  // If we get here, redisplay form
        }



        //Hashing  method
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLowerInvariant();
                return hash;
            }
        }



        // User Login
        [HttpPost]
        public IActionResult Login(Users user)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);

            if (existingUser != null && VerifyHashedPassword(existingUser.Password, user.Password))
            {
                HttpContext.Session.SetString("UserID", existingUser.UserId.ToString());
                HttpContext.Session.SetString("UserName", existingUser.Name);
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View();
        }


        private bool VerifyHashedPassword(string hashedPassword, string inputPassword)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedInputPassword = sha256.ComputeHash(Encoding.UTF8.GetBytes(inputPassword));
                return BitConverter.ToString(hashedInputPassword).Replace("-", "").ToLower() == hashedPassword;
            }
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clear session
            return RedirectToAction("Login");
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

        [HttpGet]
        public IActionResult Register()
        {
            return View();
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

        public IActionResult Cart()
        {
            return View("Cart");
        }
        public IActionResult DP()
        {
            return View("DetailedPage");
        }
        public IActionResult Checkout()
        {
            return View("Checkout");
        }


    }
}
