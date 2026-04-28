using HotelBooking.Db.Data;
using HotelBooking.Db.Interfaces;
using HotelBooking.Db.Models;
using HotelBooking.Db.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Db.Repositories;

/// <summary>
/// Repository implementation for managing review data in the database.
/// </summary>
public class ReviewRepository : Repository<Review>, IReviewRepository
{
    public ReviewRepository(HotelBookingDbContext context) : base(context) { }

    public async Task<IEnumerable<Review>> GetByHotelIdAsync(int hotelId)
    {
        return await _dbSet
            .Include(r => r.User)
            .Include(r => r.Hotel)
            .Where(r => r.HotelId == hotelId)
            .OrderByDescending(r => r.CreatedDate) // Order reviews by most recent first
            .ToListAsync();
    }

    public async Task<Review?> GetByIdWithUserAsync(int id)
    {
        return await _dbSet
            .Include(r => r.User)
            .Include(r => r.Hotel)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Review?> GetByUserAndHotelAsync(int userId, int hotelId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(r => r.UserId == userId && r.HotelId == hotelId);
    }
}
