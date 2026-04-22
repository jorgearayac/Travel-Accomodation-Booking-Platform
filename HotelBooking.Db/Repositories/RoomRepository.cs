using Microsoft.EntityFrameworkCore;
using HotelBooking.Db.Data;
using HotelBooking.Db.Interfaces;
using HotelBooking.Db.Models;

namespace HotelBooking.Db.Repositories;

public class RoomRepository : Repository<Room>, IRoomRepository
{
    public RoomRepository(HotelBookingDbContext context) : base(context) { }

    public async Task<IEnumerable<Room>> GetRoomsByIdsAsync(List<int> ids)
    {
        return await _dbSet
            .Where(r => ids.Contains(r.Id))
            .ToListAsync();
    }
}