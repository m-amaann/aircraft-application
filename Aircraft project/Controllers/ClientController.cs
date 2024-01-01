using Aircraft_project.Data;
using Aircraft_project.Models;
using Aircraft_project.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;




namespace Aircraft_project.Controllers
{
    public class ClientController : Controller
    {
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;


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




        // User Login Method
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _userService.AuthenticateUserAsync(email, password);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, email)
                    // Add additional claims as needed
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                // Set session values
                HttpContext.Session.SetString("UserID", user.UserId.ToString());
                HttpContext.Session.SetString("UserName", user.Name);

                TempData["SuccessMessage"] = "You are logged in successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View();
            }
        }




        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
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
            // return View("Shopping");
            var aircraft = _context.Aircraft.ToList();
            return View(aircraft);
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