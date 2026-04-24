using HotelBooking.Db.Models;

namespace HotelBooking.Db.Interfaces;

/// <summary>
/// Repository interface for managing hotel data in the database.
/// </summary>
public interface IHotelRepository : IRepository<Hotel>
{
    /// <summary>
    /// Retrieves all hotels along with their associated rooms.
    /// </summary>
    /// <returns>A collection of hotels with their rooms.</returns>
    Task<IEnumerable<Hotel>> GetAllWithRoomsAsync();

    /// <summary>
    /// Retrieves a hotel by its Id along with its associated rooms.
    /// </summary>
    /// <param name="id">The Id of the hotel to retrieve.</param>
    /// <returns>The hotel with its rooms, or null if not found.</returns>
    Task<Hotel?> GetByIdWithRoomsAsync(int id);

    // Pagination method to retrieve hotels with their rooms
    Task<(IEnumerable<Hotel> Items, int TotalCount)> GetPaginatedWithRoomsAsync(int pageNumber, int pageSize);

    // Search method to retrieve hotels based on search criteria with pagination
    Task<(IEnumerable<Hotel> Items, int TotalCount)> SearchAsync(
        string? query,
        decimal? minPrice,
        decimal? maxPrice,
        int? starRate,
        string? roomType,
        int adults,
        int children,
        int pageNumber,
        int pageSize);

    // Method to get Hotels with full details
    Task<Hotel?> GetByIdWithFullDetailsAsync(int id);
}
