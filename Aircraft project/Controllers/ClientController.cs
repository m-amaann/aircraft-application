using Aircraft_project.Data;
using Aircraft_project.Models;
using Aircraft_project.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(14) // Set the expiration time to 14 days
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                // Set session values
                HttpContext.Session.SetString("UserID", user.UserId.ToString());
                HttpContext.Session.SetString("UserName", user.Name);

                TempData["SuccessMessage"] = "You are logged in successfully!";

                // Directly set user ID and username in local storage using ViewData
                ViewData["UserID"] = user.UserId.ToString();
                ViewData["UserName"] = user.Name;

                return View(); // Ensure that you are returning the Login view
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

        // -----------------------------------------------------------------------

        public IActionResult Cart()
        {
            // return View("Cart");
            var cartItems = _context.Cart.Include(c => c.Aircraft).ToList();
            return View(cartItems);
        }
        [HttpGet]
        public IActionResult DetailedPage(int id)
        {
            // Fetch the aircraft details using the id
            var aircraft = _context.Aircraft.FirstOrDefault(a => a.Id == id);

            if (aircraft != null)
            {
                return View(aircraft);
            }
            else
            {
                // Handle the case where the aircraft with the given id is not found
                return NotFound();
            }
        }

        public IActionResult Checkout()
        {
            // return View("Checkout");
            var cartItems = _context.Cart.Include(c => c.Aircraft).ToList();
            return View(cartItems);
        }
        // -----------------------------------------------------------------------
        public IActionResult userProfile()
        {

            // Assuming you have a method to get the user's orders based on UserId
            var orders = _context.Orders.Where(o => o.UserId == 1).ToList();
            // var orders = _context.Orders.ToList();
            return View(orders);

        }

        [HttpGet]
        public IActionResult FilterProducts(string category, string search)
        {
            try
            {
                // Your logic to filter products based on the category and search
                var filteredProducts = _context.Aircraft
                    .Where(a => (string.IsNullOrEmpty(category) || a.Category == category) &&
                                (string.IsNullOrEmpty(search) || a.Name.Contains(search)))
                    .ToList();

                // Return the view with the filtered products
                return View("Shopping", filteredProducts);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"Exception in FilterProducts: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        public IActionResult AddToCart(Cart model)
        {
            try
            {
                // Check if any required fields are empty
                if (model.AircraftId == 0 || model.Quantity <= 0 || string.IsNullOrEmpty(model.Color)
            || string.IsNullOrEmpty(model.Engine)
            || string.IsNullOrEmpty(model.FanType))
                {
                    TempData["ErrorMessage"] = "Aircraft, Quantity, Color, Engine, SeatsSize, and FanType are required fields.";
                    return RedirectToAction("Shopping");
                }

                Console.WriteLine($"Received Cart Model: {Newtonsoft.Json.JsonConvert.SerializeObject(model)}");

                var cartItem = new Cart
                {
                    UserId = 1,  // You need to implement a method to get the user ID
                    AircraftId = model.AircraftId,
                    Quantity = model.Quantity,
                    Color = model.Color,
                    Engine = model.Engine,
                    SeatsSize = model.SeatsSize,
                    FanType = model.FanType
                    // Add other properties as needed
                };

                _context.Cart.Add(cartItem);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Item added to the cart successfully!";
                return RedirectToAction("Shopping");
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"Exception in AddToCart: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int cartItemId)
        {
            try
            {
                var cartItem = _context.Cart.FirstOrDefault(c => c.CartId == cartItemId);

                if (cartItem == null)
                {
                    TempData["ErrorMessage"] = "Cart item not found.";
                    return RedirectToAction("Cart");
                }

                _context.Cart.Remove(cartItem);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Item removed from the cart successfully!";
                return RedirectToAction("Cart");
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"Exception in RemoveFromCart: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}