using HotelBooking.Db.Models;

namespace HotelBooking.Db.Interfaces;

/// <summary>
/// Hotel image repository for managing data in the database.
/// </summary>
public interface IHotelImageRepository : IRepository<HotelImage>
{
    /// <summary>
    /// Retrieves all images for a specific hotel by its Id.
    /// </summary>
    /// <param name="hotelId">The Id of the hotel to search images for.</param>
    /// <returns>A collection of HotelImage.</returns>
    Task<IEnumerable<HotelImage>> GetByHotelIdAsync(int hotelId);
}
