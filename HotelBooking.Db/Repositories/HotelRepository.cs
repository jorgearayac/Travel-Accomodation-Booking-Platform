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

    public async Task<(IEnumerable<Hotel> Items, int TotalCount)> GetPaginatedWithRoomsAsync(int pageNumber, int pageSize)
    {
        var totalCount = await _dbSet.CountAsync();
        var items = await _dbSet
            .Include(h => h.Rooms)
            .OrderBy(h => h.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (items, totalCount);
    }
}
