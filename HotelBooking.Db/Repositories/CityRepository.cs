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
        return await _dbSet.Include(c => c.Hotels)
            .ToListAsync();
    }

    public async Task<City?> GetByIdWithHotelsAsync(int id)
    {
        return await _dbSet.Include(c => c.Hotels)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}