using LawnScheduler.Data; // Ensure this is included for ApplicationDbContext
using LawnScheduler.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LawnScheduler.Controllers
{
    public class ConflictManagerController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly CustomDbContext _context;
        private readonly ApplicationDbContext _applicationContext; 

        public ConflictManagerController(IBookingService bookingService, CustomDbContext context, ApplicationDbContext applicationContext) 
        {
            _bookingService = bookingService;
            _context = context;
            _applicationContext = applicationContext; 
        }



        // View conflicting bookings
        public async Task<IActionResult> Index()
        {
            var conflictingBookings = await _bookingService.GetConflictingBookingsAsync();

            var customerEmails = await _applicationContext.Users.Where(u => conflictingBookings.Select(b => b.CustomerId).Contains(u.Id)).ToDictionaryAsync(u => u.Id, u => u.Email);

            ViewBag.AvailableMachines = await _context.Machines.ToListAsync();
            ViewBag.CustomerEmails = customerEmails;

            return View(conflictingBookings);
        }




        //Getting all bookings
        public async Task<IActionResult> AllBookings()
        {
            var allBookings = await _context.Bookings.Include(b => b.Machine).Where(b => !b.IsConflict).ToListAsync();
            var customerEmails = await _applicationContext.Users.Where(u => allBookings.Select(b => b.CustomerId).Contains(u.Id)).ToDictionaryAsync(u => u.Id, u => u.Email);

            ViewBag.CustomerEmails = customerEmails;

            return View(allBookings);
        }





        // Resolve a conflict post method
        [HttpPost]
        public async Task<IActionResult> ResolveConflict(int bookingId, int newMachineId)
        {
            var isResolved = await _bookingService.ResolveConflictAsync(bookingId, newMachineId);

            if (!isResolved)
            {
                return BadRequest("The selected machine is not available on the requested date.");
            }

            return RedirectToAction("Index"); 
        }
    }
}
