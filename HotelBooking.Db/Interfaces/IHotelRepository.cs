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

    /// <summary>
    /// Paginates a hotel with details of its rooms.
    /// </summary>
    /// <param name="pageNumber">The number of the page.</param>
    /// <param name="pageSize">The size of the page.</param>
    /// <returns>A collection of Hotels with details of its rooms, and pagination information.</returns>
    Task<(IEnumerable<Hotel> Items, int TotalCount)> GetPaginatedWithRoomsAsync(int pageNumber, int pageSize);

    /// <summary>
    /// Searchs a hotel based on various criteria.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="minPrice">Minimum price to search for.</param>
    /// <param name="maxPrice">Maximum price to search for.</param>
    /// <param name="starRate">Star rating (1-5) to search for.</param>
    /// <param name="roomType">Room type (budget-luxury-boutique) to search for.</param>
    /// <param name="adults">Number of adults (default = 2).</param>
    /// <param name="children">Number of children (default = 0).</param>
    /// <param name="pageNumber">The number of the page.</param>
    /// <param name="pageSize">The size of the page.</param>
    /// <returns>A collection of Hotels.</returns>
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

    /// <summary>
    /// Retrieves a hotel with full details by its Id.
    /// </summary>
    /// <param name="id">The Id of the hotel to search for.</param>
    /// <returns>The Hotel with full details, null if not found.</returns>
    Task<Hotel?> GetByIdWithFullDetailsAsync(int id);
}
