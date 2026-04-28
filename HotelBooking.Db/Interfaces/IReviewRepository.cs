using HotelBooking.Db.Models;

namespace HotelBooking.Db.Interfaces;

/// <summary>
/// Review repository for managing data in the database.
/// </summary>
public interface IReviewRepository : IRepository<Review>
{
    /// <summary>
    /// Retrieves all reviews for a specific hotel
    /// </summary>
    /// <param name="hotelId">The Id of the hotel to search reviews for.</param>
    /// <returns>A collection of Reviews for a hotel, null otherwise.</returns>
    Task<IEnumerable<Review>> GetByHotelIdAsync(int hotelId);

    /// <summary>
    /// Retrieves a review by its Id.
    /// </summary>
    /// <param name="id">The Id of the review to search for.</param>
    /// <returns>Review, null if not found.</returns>
    Task<Review?> GetByIdWithUserAsync(int id);

    // Get a review by its user Id and hotel Id (to check if a user has already reviewed a hotel)
    /// <summary>
    /// Retrieves all reviews made by a user, and the hotel reviewed.
    /// </summary>
    /// <param name="userId">The Id of the user to search reviews for.</param>
    /// <param name="hotelId">The Id of the hotel to check.</param>
    /// <returns>Review, null if not found.</returns>
    Task<Review?> GetByUserAndHotelAsync(int userId, int hotelId);
}
