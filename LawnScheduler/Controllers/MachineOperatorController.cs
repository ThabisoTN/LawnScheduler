using LawnScheduler.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.Linq;
using System.Threading.Tasks;

namespace LawnScheduler.Controllers
{
    public class MachineOperatorController : Controller
    {
        private readonly CustomDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _applicationContext; 

        public MachineOperatorController(CustomDbContext context, UserManager<IdentityUser> userManager, ApplicationDbContext applicationContext) // Update constructor
        {
            _context = context;
            _userManager = userManager;
            _applicationContext = applicationContext; 
        }




        //get all bookings 
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var bookings = await _context.Bookings.Include(b => b.Machine).Where(b => b.Machine.OperatorId == userId && !b.IsConflict).ToListAsync();
            var customerEmails = await _applicationContext.Users.Where(u => bookings.Select(b => b.CustomerId).Contains(u.Id)).ToDictionaryAsync(u => u.Id, u => u.Email);

            ViewBag.CustomerEmails = customerEmails;

            return View(bookings);
        }





        // Acknowledging post method
        [HttpPost]
        public async Task<IActionResult> AcknowledgeBooking(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking != null && !booking.IsConfirmed)
            {
                booking.IsConfirmed = true;
                booking.Status = "Booking Acknowledged";
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }





        // updating work progress. 
        [HttpPost]
        public async Task<IActionResult> CompleteBooking(int bookingId, Machine obj)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking != null && booking.IsConfirmed)
            {
                booking.Status = "Work Completed";
                booking.CompletionDate = DateTime.Now;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> RegisterMachine()
        {
            return View();

        }


        [HttpPost]
        public async Task<IActionResult> RegisterMachine(Machine obj) 
        {
            var OperatorUserId = _userManager.GetUserId(User);
            if (OperatorUserId == null)
            {
                throw new Exception("User not recognized");
            }


            var machine = new Machine
            {
                OperatorId = OperatorUserId,
                MachineId = obj.MachineId,
                Model = obj.Model,
                Description = obj.Description,
                MachineName = obj.MachineName,

            };

            _context.Machines.Add(machine);
            await _context.SaveChangesAsync();
            Console.WriteLine("Machine added successfull");

            return RedirectToAction("Index", new {Message="Machine added successful!!!"});

        }
    }
}
