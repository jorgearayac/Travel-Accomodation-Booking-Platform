using HotelBooking.Db.Data;
using HotelBooking.Db.Interfaces;
using HotelBooking.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Db.Repositories;

public class HotelImageRepository : Repository<HotelImage>, IHotelImageRepository
{
    public HotelImageRepository(HotelBookingDbContext context) : base(context) { }

    public async Task<IEnumerable<HotelImage>> GetByHotelIdAsync(int hotelId)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(hi => hi.HotelId == hotelId)
            .ToListAsync();
    }
}
