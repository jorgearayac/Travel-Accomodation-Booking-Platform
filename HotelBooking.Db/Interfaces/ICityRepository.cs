using HotelBooking.Db.Models;

namespace HotelBooking.Db.Interfaces;

/// <summary>
/// Repository interface for managing city data in the database.
/// </summary>
public interface ICityRepository : IRepository<City>
{
    /// <summary>
    /// Retrieves all cities, including their associated hotels.
    /// </summary>
    /// <returns>A collection of cities with their associated hotels.</returns>
    Task<IEnumerable<City>> GetAllWithHotelsAsync();

    /// <summary>
    /// Retrieves a city by its Id, including its associated hotels.
    /// </summary>
    /// <param name="id">The Id of the city to retrieve.</param>
    /// <returns>The city with its associated hotels, or null if not found.</returns>
    Task<City?> GetByIdWithHotelsAsync(int id);

    /// <summary>
    /// Paginates a hotel data.
    /// </summary>
    /// <param name="pageNumber">The number of the page.</param>
    /// <param name="pageSize">The size of the page.</param>
    /// <returns>A collection of cities with their details, and pagination details.</returns>
    Task<(IEnumerable<City> Items, int TotalCount)> GetPaginatedWithHotelsAsync(int pageNumber, int pageSize);

    /// <summary>
    /// Retrieves the top booked cities by a count.
    /// </summary>
    /// <param name="count">The number to set the top of cities.</param>
    /// <returns>A collection of cities ranked by top bookings.</returns>
    Task<IEnumerable<City>> GetTopBookedCitiesAsync(int count);
}