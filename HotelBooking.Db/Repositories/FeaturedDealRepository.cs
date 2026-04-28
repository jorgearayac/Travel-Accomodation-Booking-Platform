using HotelBooking.Db.Data;
using HotelBooking.Db.Interfaces;
using HotelBooking.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Db.Repositories;

public class FeaturedDealRepository : Repository<FeaturedDeal>, IFeaturedDealRepository
{
    public FeaturedDealRepository(HotelBookingDbContext context) : base(context) { }

    public async Task<IEnumerable<FeaturedDeal>> GetAllWithHotelAndCityAsync()
    {
        return await _dbSet
            .Include(fd => fd.Hotel)
            .ThenInclude(h => h.City)
            .ToListAsync();
    }

    public async Task<FeaturedDeal?> GetByIdWithHotelAndCityAsync(int id)
    {
        return await _dbSet
            .Include(fd => fd.Hotel)
            .ThenInclude(h => h.City)
            .FirstOrDefaultAsync(fd => fd.Id == id);
    }
}