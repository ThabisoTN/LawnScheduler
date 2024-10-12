using LawnScheduler.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LawnScheduler.Controllers
{
    public class MachineOperatorController : Controller
    {
        private readonly CustomDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MachineOperatorController(CustomDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var bookings = await _context.Bookings.Include(b => b.Machine).Where(b => b.Machine.OperatorId == userId).ToListAsync();

            return View(bookings);
        }


        //acknowleding post method
        [HttpPost]
        public async Task<IActionResult> AcknowledgeBooking(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking != null && !booking.IsConfirmed)
            {
                booking.IsConfirmed = true; 
                booking.Status = "In Progress"; 
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }


        //Setting done with work for the booking. 
        [HttpPost]
        public async Task<IActionResult> CompleteBooking(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking != null && booking.IsConfirmed) 
            {
                booking.Status = "Completed"; 
                booking.CompletionDate = DateTime.Now; 
                await _context.SaveChangesAsync(); 
            }

            return RedirectToAction("Index"); 
        }

    }
}
