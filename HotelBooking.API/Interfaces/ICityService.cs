using HotelBooking.API.DTOs.Cities;

namespace HotelBooking.API.Interfaces;

/// <summary>
/// Interface for city-related operations in the hotel booking service.
/// </summary>
public interface ICityService
{
    /// <summary>
    /// Retrieves a list of all the cities available in the system.
    /// </summary>
    /// <returns>A collection of <see cref="CityResponse"/> objects representing the cities.</returns>
    Task<IEnumerable<CityResponse>> GetAllCitiesAsync();

    /// <summary>
    /// Retrieves a city by its Id.
    /// </summary>
    /// <param name="id">The Id of the city to search for.</param>
    /// <returns>A <see cref="CityResponse"/> object representing the city.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no city with the specified Id is found.</exception>
    Task<CityResponse> GetCityByIdAsync(int id);
    
    /// <summary>
    /// Creates a new city in the system.
    /// </summary>
    /// <param name="request">The request object containing city details.</param>
    /// <returns>A <see cref="CityResponse"/> object representing the created city.</returns>
    Task<CityResponse> CreateCityAsync(CityRequest request);

    /// <summary>
    /// Updates an existing city in the system.
    /// </summary>
    /// <param name="id">The Id of the city to update.</param>
    /// <param name="request">The request object containing updated city details.</param>
    /// <returns>A <see cref="CityResponse"/> object representing the updated city.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no city with the specified Id is found.</exception>

    Task<CityResponse> UpdateCityAsync(int id, CityRequest request);

    /// <summary>
    /// Deletes a city from the system.
    /// </summary>
    /// <param name="id">The Id of the city to delete.</param>
    /// <exception cref="KeyNotFoundException">Thrown when no city with the specified Id is found.</exception>
    Task DeleteCityAsync(int id);
}
