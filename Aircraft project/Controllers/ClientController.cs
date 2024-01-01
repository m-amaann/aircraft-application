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
using Aircraft_project.Services;




namespace Aircraft_project.Controllers
{
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;


        //Constructor
        public ClientController(ApplicationDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;

        }

        // Register Method
        [HttpPost]
        public async Task<IActionResult> Register(Users user, string ConfirmPassword)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var result = await _userService.RegisterUserAsync(user, ConfirmPassword);
            if (result.Success)
            {
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
            return View(user);
        }



/*        //Hashing  method
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLowerInvariant();
                return hash;
            }
        }*/




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
