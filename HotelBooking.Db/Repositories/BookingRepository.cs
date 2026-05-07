using HotelBooking.Db.Data;
using HotelBooking.Db.Interfaces;
using HotelBooking.Db.Models;
using Microsoft.EntityFrameworkCore;
namespace HotelBooking.Db.Repositories;

public class BookingRepository : Repository<Booking>, IBookingRepository
{
    public BookingRepository(HotelBookingDbContext context) : base(context) { }

    public async Task<Booking?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(b => b.BookingRooms)
            .ThenInclude(br => br.Room)
            .ThenInclude(r => r.Hotel)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Booking>> GetByUserIdAsync(int userId)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(b => b.BookingRooms)
            .ThenInclude(br => br.Room)
            .ThenInclude(r => r.Hotel)
            .ThenInclude(h => h.City)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.CreatedDate)
            .ToListAsync();
    }
}