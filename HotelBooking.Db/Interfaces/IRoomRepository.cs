using HotelBooking.Db.Models;

namespace HotelBooking.Db.Interfaces;

public interface IRoomRepository : IRepository<Room>
{
    Task<IEnumerable<Room>> GetRoomsByIdsAsync(List<int> ids);
}
