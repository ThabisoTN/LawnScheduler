using LawnScheduler.Data;
using LawnScheduler.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LawnScheduler.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CustomDbContext _context; // Add this line

        public HomeController(ILogger<HomeController> logger, CustomDbContext context) // Add context parameter
        {
            _logger = logger;
            _context = context; // Assign the context to the field
        }

        public IActionResult Index()
        {
            var machines = _context.Machines.ToList(); // Fetching machines from the database
            return View(machines); // Pass machines to the view
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
