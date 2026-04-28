using HotelBooking.API.DTOs.FeaturedDeals;

namespace HotelBooking.API.Interfaces;

/// <summary>
/// Interface for deal-related operations in the system.
/// </summary>
public interface IFeaturedDealService
{
    /// <summary>
    /// Retrieves all featured deals.
    /// </summary>
    /// <returns>A collection of featured deals, null otherwise.</returns>
    Task<IEnumerable<FeaturedDealResponse>> GetAllFeaturedDealsAsync();

    /// <summary>
    /// Retrieves a featured deal by its Id.
    /// </summary>
    /// <param name="id">The Id of the featured deal to search for.</param>
    /// <returns>FeaturedDealResponse, null otherwise.</returns>
    Task<FeaturedDealResponse?> GetFeaturedDealByIdAsync(int id);

    /// <summary>
    /// Creates a featured deal.
    /// </summary>
    /// <param name="request">The featured deal details.</param>
    /// <returns>FeaturedDealResponse.</returns>
    Task<FeaturedDealResponse> CreateFeaturedDealAsync(FeaturedDealRequest request);

    /// <summary>
    /// Updates a featured deal by its Id.
    /// </summary>
    /// <param name="id">The Id of the featured deal to update.</param>
    /// <param name="request">The details of the updated featured deal.</param>
    /// <returns>FeaturedDealResponse, null otherwise.</returns>
    Task<FeaturedDealResponse?> UpdateFeaturedDealAsync(int id, FeaturedDealRequest request);

    /// <summary>
    /// Deletes a featured deal by its Id.
    /// </summary>
    /// <param name="id">The Id of the featured deal to delete.</param>
    /// <returns></returns>
    Task DeleteFeaturedDealAsync(int id);
}
