using HotelBooking.Db.Data;
using HotelBooking.Db.Interfaces;
using HotelBooking.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Db.Repositories;

public class HotelRepository : Repository<Hotel>, IHotelRepository
{
    public HotelRepository(HotelBookingDbContext context) : base(context) { }

    public async Task<IEnumerable<Hotel>> GetAllWithRoomsAsync()
    {
        return await _dbSet.Include(h => h.Rooms)
            .ToListAsync();
    }

    public async Task<Hotel?> GetByIdWithRoomsAsync(int id)
    {
        return await _dbSet.Include(h => h.Rooms)
            .FirstOrDefaultAsync(h => h.Id == id);
    }
}
