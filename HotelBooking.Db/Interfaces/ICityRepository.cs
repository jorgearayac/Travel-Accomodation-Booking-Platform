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
}