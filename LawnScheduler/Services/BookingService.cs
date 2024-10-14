using LawnScheduler.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace LawnScheduler.Services
{
    public class BookingService : IBookingService
    {
        private readonly CustomDbContext _context;

        public BookingService(CustomDbContext context)
        {
            _context = context;
        }


        //Book mechine method
        public async Task<Booking?> BookMachineAsync(string userId, int machineId, DateTime scheduledDate)
        {
            try
            {
                var machine = await _context.Machines.FindAsync(machineId);

                if (machine != null)
                {
            
                    var existingBookings = await _context.Bookings.Where(b => b.MachineId == machineId && b.ScheduledDate.Date == scheduledDate.Date).ToListAsync();

                    
                    if (existingBookings.Any())
                    {
                        
                        bool allCompleted = existingBookings.All(b => b.Status == "Completed");

                        
                        if (allCompleted)
                        {
                           
                            var newBooking = new Booking
                            {
                                CustomerId = userId,
                                MachineId = machineId,
                                BookingDate = DateTime.Now,
                                ScheduledDate = scheduledDate,
                                IsConfirmed = false,
                                IsConflict = false 
                            };

                            _context.Bookings.Add(newBooking);
                            await _context.SaveChangesAsync();

                            return newBooking;
                        }
                        else
                        {
                            
                            var conflictBooking = new Booking
                            {
                                CustomerId = userId,
                                MachineId = machineId,
                                BookingDate = DateTime.Now,
                                ScheduledDate = scheduledDate,
                                IsConfirmed = false,
                                IsConflict = true
                            };

                            _context.Bookings.Add(conflictBooking);
                            await _context.SaveChangesAsync();

                            return null;
                        }
                    }
                    else
                    {
                      
                        var newBooking = new Booking
                        {
                            CustomerId = userId,
                            MachineId = machineId,
                            BookingDate = DateTime.Now,
                            ScheduledDate = scheduledDate,
                            IsConfirmed = false,
                            IsConflict = false 
                        };

                        _context.Bookings.Add(newBooking);
                        await _context.SaveChangesAsync();

                        return newBooking;
                    }
                }

                return null; 
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while booking the machine.", ex);
            }
        }


        //Retrieving conflicting bookings
        public async Task<List<Booking>> GetConflictingBookingsAsync()
        {
            return await _context.Bookings.Where(b => b.IsConflict).ToListAsync();
        }



        //Resolve conflict method
        public async Task<bool> ResolveConflictAsync(int bookingId, int newMachineId)
        {
           
            var existingBooking = await _context.Bookings.FindAsync(bookingId);
            if (existingBooking == null)
            {
                return false;
            }

            
            var isMachineAvailable = !await _context.Bookings.AnyAsync(b => b.MachineId == newMachineId && b.ScheduledDate.Date == existingBooking.ScheduledDate.Date);

            if (!isMachineAvailable)
            {
                return false; 
            }

            
            existingBooking.MachineId = newMachineId; 
            existingBooking.IsConflict = false; 
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
