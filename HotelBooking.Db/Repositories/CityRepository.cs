using HotelBooking.Db.Data;
using HotelBooking.Db.Interfaces;
using HotelBooking.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Db.Repositories;

/// <summary>
/// Repository implementation for managing city data in the database.
/// </summary>
public class CityRepository : Repository<City>, ICityRepository
{
    public CityRepository(HotelBookingDbContext context) : base(context) { }

    public async Task<IEnumerable<City>> GetAllWithHotelsAsync()
    {
        return await _dbSet
            .AsNoTracking()
            .Include(c => c.Hotels)
            .ToListAsync();
    }

    public async Task<City?> GetByIdWithHotelsAsync(int id)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(c => c.Hotels)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<(IEnumerable<City> Items, int TotalCount)> GetPaginatedWithHotelsAsync(int pageNumber, int pageSize)
    {
        var totalCount = await _dbSet.CountAsync();
        var items = await _dbSet
            .AsNoTracking()
            .Include(c => c.Hotels)
            .OrderBy(c => c.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (items, totalCount);
    }

    public async Task<IEnumerable<(City City, int BookingCount)>> GetTopBookedCitiesAsync(int count)
    {
        return await _dbSet
            .Select(c => new
            {
                City = c,
                BookingCount = c.Hotels
                    .SelectMany(h => h.Rooms)
                    .SelectMany(r => r.BookingRooms)
                    .Count()
            })
            .OrderByDescending(x => x.BookingCount)
            .Take(count)
            .AsNoTracking()
            .Select(x => new { x.City, x.BookingCount })
            .ToListAsync()
            .ContinueWith(t => t.Result.Select(x => (x.City, x.BookingCount)));
    }
}