using LawnScheduler.Data;
using System;
using System.Threading.Tasks;

namespace LawnScheduler.Services
{
    public interface IBookingService
    {
        Task<Booking?> BookMachineAsync(string userId, int machineId, DateTime scheduledDate);
        Task<List<Booking>> GetConflictingBookingsAsync(); 
        Task<bool> ResolveConflictAsync(int bookingId, int newMachineId); 
    }
}
