using LawnScheduler.Data; // Make sure this contains the ApplicationUser
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
        private readonly UserManager<IdentityUser> _userManager; 


        public CustomerController(CustomDbContext context, IBookingService bookingService, UserManager<IdentityUser> userManager)
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

        public IActionResult BookingConfirmation()
        {
            return View();
        }

        public IActionResult BookingConflict()
        {
            return View();
        }


        // New method to get all bookings for the customer
        // New method to get all bookings for the customer
        public async Task<IActionResult> MyBookings()
        {
            var userId = _userManager.GetUserId(User);
            var bookings = await _context.Bookings
                .Include(b => b.Machine) // Include machine details
                .Where(b => b.CustomerId == userId)
                .ToListAsync();

            // Retrieve operator emails and add to the bookings
            var operatorIds = bookings.Select(b => b.Machine.OperatorId).Distinct().ToList();
            var operators = await _userManager.Users
                .Where(u => operatorIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => u.Email);

            ViewBag.OperatorEmails = operators; // Store operator emails in ViewBag

            return View(bookings);
        }

    }
}
