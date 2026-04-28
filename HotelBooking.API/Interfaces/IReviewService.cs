using HotelBooking.API.DTOs.Reviews;

namespace HotelBooking.API.Interfaces;

/// <summary>
/// Interface for review-related operations in the system.
/// </summary>
public interface IReviewService
{
    /// <summary>
    /// Gets all reviews for a specific hotel by its Id.
    /// </summary>
    /// <param name="hotelId"></param>
    /// <returns></returns>
    Task<IEnumerable<ReviewResponse>> GetReviewsByHotelIdAsync(int hotelId);

    /// <summary>
    /// Creates a new review for a hotel. The user can only create one review per hotel.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<ReviewResponse> CreateReviewAsync(int userId, CreateReviewRequest request);

    /// <summary>
    /// Deletes a review by its Id. Only the user who created the review can delete it.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="reviewId"></param>
    /// <returns></returns>
    Task DeleteReviewAsync(int userId, int reviewId);
}
