using HotelBooking.Db.Models;

namespace HotelBooking.Db.Interfaces;

public interface IBookingRepository : IRepository<Booking>
{
    Task<Booking?> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<Booking>> GetByUserIdAsync(int userId);
}
