using Aircraft_project.Data;
using Aircraft_project.Models;
using Aircraft_project.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
        // public IActionResult userProfile()
        // {
        //     // Retrieve user ID from local storage
        //     var localStorageUserId = HttpContext.Session.GetString("UserID");

        //     if (localStorageUserId != null)
        //     {
        //         // Convert the user ID to an integer
        //         if (int.TryParse(localStorageUserId, out int userId))
        //         {
        //             // Assuming you have a method to get user details based on UserId
        //             var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

        //             if (user != null)
        //             {
        //                 // Assuming you have a method to get the user's orders based on UserId
        //                 var orders = _context.Orders.Where(o => o.UserId == userId).ToList();

        //                 // Pass user and orders to the view
        //                 ViewData["UserDetails"] = user;
        //                 return View(orders);
        //             }
        //         }
        //     }

        //     // Redirect to login if user ID is not found or invalid
        //     return RedirectToAction("Login");
        // }

        // Fetch orders for the current user
        private List<Orders> GetUserOrders(int userId)
        {
            return _context.Orders.Where(o => o.UserId == userId).ToList();
        }

        public IActionResult userProfile()
        {
            // Retrieve user ID from local storage
            var localStorageUserId = HttpContext.Session.GetString("UserID");

            if (localStorageUserId != null && int.TryParse(localStorageUserId, out int userId))
            {
                var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

                if (user != null)
                {
                    var orders = GetUserOrders(userId);

                    ViewData["UserDetails"] = user;
                    ViewData["UserId"] = userId;
                    ViewBag.Orders = orders;

                    return View();
                }
            }

            return RedirectToAction("Login");
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
                if (model.AircraftId == 0 || model.Quantity <= 0 || string.IsNullOrEmpty(model.Color) || string.IsNullOrEmpty(model.Engine) || string.IsNullOrEmpty(model.FanType))
                {
                    TempData["ErrorMessage"] = "Aircraft, Quantity, Color, Engine, SeatsSize, and FanType are required fields.";
                    return RedirectToAction("Shopping");
                }

                Console.WriteLine($"Received Cart Model: {Newtonsoft.Json.JsonConvert.SerializeObject(model)}");

                var cartItem = new Cart
                {
                    UserId = model.UserId,  // You need to implement a method to get the user ID
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

        [HttpPost]
        public IActionResult UpdateProfile(Users updatedUser)
        {
            try
            {
                // Check if the model is valid
                // if (!ModelState.IsValid)
                // {
                //     Console.WriteLine("Model state is not valid.");
                //     // If the model state is not valid, return the view with the model to display validation errors
                //     return View("userProfile");
                // }

                // Retrieve user ID from local storage
                var localStorageUserId = HttpContext.Session.GetString("UserID");
                Console.WriteLine($"Local storage user ID: {localStorageUserId}");

                if (localStorageUserId != null && int.TryParse(localStorageUserId, out int userId))
                {
                    Console.WriteLine($"Parsed user ID: {userId}");

                    // Ensure that the user making the request is the same as the one being updated
                    if (userId != updatedUser.UserId)
                    {
                        Console.WriteLine("User ID mismatch. Returning 403 Forbidden.");
                        return Forbid(); // Return 403 Forbidden if unauthorized
                    }

                    // Get the existing user details
                    var existingUser = _context.Users.Find(userId);

                    if (existingUser == null)
                    {
                        Console.WriteLine("Existing user not found. Returning NotFound.");
                        return NotFound();
                    }

                    // Update the user details
                    existingUser.Name = updatedUser.Name;
                    existingUser.MobileNumber = updatedUser.MobileNumber;
                    existingUser.Address = updatedUser.Address;
                    existingUser.Image = updatedUser.Image;

                    // Save changes to the database
                    _context.SaveChanges();
                    Console.WriteLine("User profile updated successfully!");

                    TempData["SuccessMessage"] = "Profile updated successfully!";
                    return RedirectToAction("userProfile");
                }

                Console.WriteLine("User ID not found or invalid. Redirecting to Login.");
                return RedirectToAction("Login"); // Redirect to login if user ID is not found or invalid
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"Exception in UpdateProfile: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        public IActionResult ContactForm(Contact contact)
        {
            if (ModelState.IsValid)
            {
                // Save the contact information to the database
                var newContact = new Contact
                {
                    Name = contact.Name,
                    Email = contact.Email,
                    Subject = contact.Subject,
                    Message = contact.Message
                };

                _context.Contact.Add(newContact);
                _context.SaveChanges();

                // Optionally, you can redirect to a thank-you page or return a success message
                TempData["SuccessMessage"] = "Your message has been sent. Thank you!";
                return RedirectToAction("Index"); // Replace with the appropriate action
            }

            // If ModelState is not valid, return to the contact page with validation errors
            return View("Contact"); // Assuming you have a view named "Contact"
        }
    }
}