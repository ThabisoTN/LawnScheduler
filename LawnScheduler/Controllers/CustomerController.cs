using LawnScheduler.Data; 
using LawnScheduler.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LawnScheduler.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CustomDbContext _context;
        private readonly IBookingService _bookingService;
        private readonly UserManager<ApplicationUser> _userManager; 


        public CustomerController(CustomDbContext context, IBookingService bookingService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _bookingService = bookingService;
            _userManager = userManager;
        }


        //List all machines
        public IActionResult Index()
        {
            var machines = _context.Machines.ToList();
            return View(machines); 
        }

        [HttpPost]
        public async Task<IActionResult> BookMachine(int machineId, DateTime scheduledDate)
        {
            var userId = _userManager.GetUserId(User);
            var bookingResult = await _bookingService.BookMachineAsync(userId, machineId, scheduledDate);

            if (bookingResult == null)
            {
                return RedirectToAction("BookingConflict");
            }

            return RedirectToAction("BookingConfirmation");
        }


        //get booking confirmation 
        public IActionResult BookingConfirmation()
        {
            return View();
        }

        //get  booking conflict 

        public IActionResult BookingConflict()
        {
            return View();
        }




        // Method to get all bookings for the customer
        public async Task<IActionResult> MyBookings()
        {
            var userId = _userManager.GetUserId(User);

            // Fetch bookings and include related machine data
            var bookings = await _context.Bookings
                .Include(b => b.Machine)
                .Where(b => b.CustomerId == userId)
                .ToListAsync();

            // Get distinct operator IDs from bookings
            var operatorIds = bookings
                .Select(b => b.Machine.OperatorId)
                .Distinct()
                .ToList();

            // Fetch operators' names and store them in a dictionary
            var operators = await _userManager.Users
                .Where(u => operatorIds.Contains(u.Id))
                .ToDictionaryAsync(
                    u => u.Id,
                    u => $"{u.FirstName} {u.LastName}"
                );

            // Pass operator names to the view using ViewBag
            ViewBag.OperatorNames = operators;

            return View(bookings);
        }


    }
}
