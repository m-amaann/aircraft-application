using Aircraft_project.Data;
using Aircraft_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Add this line for ILogger

namespace Aircraft_project.Controllers
{
    public class AircraftController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AircraftController> _logger; // Add this line for ILogger

        public AircraftController(ApplicationDbContext context, ILogger<AircraftController> logger) // Add ILogger parameter
        {
            _context = context;
            _logger = logger; // Initialize _logger
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddAircraft(Aircraft aircraft)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Aircraft.Add(aircraft);
                    _context.SaveChanges();
                    _logger.LogInformation("Aircraft added successfully.");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error adding aircraft to the database.");
                    ModelState.AddModelError(string.Empty, "An error occurred while saving changes.");
                    return View(aircraft);
                }
            }

            // Log ModelState errors
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    _logger.LogError($"ModelState Error: {error.ErrorMessage}");
                }
            }

            return View(aircraft);
        }
    }
}
