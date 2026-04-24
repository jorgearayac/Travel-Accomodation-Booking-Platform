using HotelBooking.API.DTOs.Hotels;
using HotelBooking.API.DTOs.Pagination;
using HotelBooking.API.DTOs.Search;

namespace HotelBooking.API.Interfaces;

/// <summary>
/// Interface for hotel-related operations in the hotel booking service.
/// </summary>
public interface IHotelService
{
    /// <summary>
    /// Retrieves a list of all hotels available in the system with pagination.
    /// </summary>  
    /// <returns>A collection of <see cref="HotelResponse"/>.</returns>
    Task<PaginationResponse<HotelResponse>> GetAllHotelsAsync(PaginationRequest pagination);

    /// <summary>
    /// Retrieves a hotel by its Id.
    /// </summary>
    /// <param name="id">The Id of the hotel to retrieve.</param>
    /// <returns>The <see cref="HotelResponse"/>.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no hotel with the specified Id is found.</exception>
    Task<HotelResponse> GetHotelByIdAsync(int id);

    /// <summary>
    /// Creates a new hotel in the system.
    /// </summary>
    /// <param name="request">The hotel request containing the details of the hotel to create.</param>
    /// <returns>The created <see cref="HotelResponse"/>.</returns>
    Task<HotelResponse> CreateHotelAsync(HotelRequest request);

    /// <summary>
    /// Updates an existing hotel in the system.
    /// </summary>
    /// <param name="id">The Id of the hotel to update.</param>
    /// <param name="request">The hotel request containing the updated details.</param>
    /// <returns>The updated <see cref="HotelResponse"/>.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no hotel with the specified Id is found.</exception>
    Task<HotelResponse> UpdateHotelAsync(int id, HotelRequest request);

    /// <summary>
    /// Deletes a hotel by its Id.
    /// </summary>
    /// <param name="id">The Id of the hotel to delete.</param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException">Thrown when no hotel with the specified Id is found.</exception>"
    Task DeleteHotelAsync(int id);

    /// <summary>
    /// Searches for hotels based on the specified search criteria.
    /// </summary>
    /// <param name="request">The search request containing the search criteria.</param>
    /// <returns>A paginated response containing the search results.</returns>
    Task<PaginationResponse<HotelResponse>> SearchHotelsAsync(HotelSearchRequest request);
}