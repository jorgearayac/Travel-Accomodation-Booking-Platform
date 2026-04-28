using HotelBooking.Db.Models;

namespace HotelBooking.Db.Interfaces;

/// <summary>
/// Room repository for managing data in the database.
/// </summary>
public interface IRoomRepository : IRepository<Room>
{
    /// <summary>
    /// Retrieves all rooms in a list of Id.
    /// </summary>
    /// <param name="ids">A list of Id.</param>
    /// <returns>A collection of rooms, null otherwise.</returns>
    Task<IEnumerable<Room>> GetRoomsByIdsAsync(List<int> ids);
}
