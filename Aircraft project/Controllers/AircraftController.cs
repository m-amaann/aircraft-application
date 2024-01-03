using Aircraft_project.Data;
using Aircraft_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Aircraft_project.Controllers
{
    public class AircraftController : Controller
    {
        private readonly ApplicationDbContext _context;



        //Constructor
        public AircraftController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddAircraft(Aircraft model)
        {
            if (ModelState.IsValid)
            {
                _context.Aircraft.Add(model); // Add into Database
                _context.SaveChanges();

                return RedirectToAction("Aircraft", "Dashboard");

            }
            return View(model);
        }
    }
}
