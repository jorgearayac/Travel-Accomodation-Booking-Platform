using HotelBooking.Db.Models;

namespace HotelBooking.Db.Interfaces;

/// <summary>
/// Booking repository for managing data in the database.
/// </summary>
public interface IBookingRepository : IRepository<Booking>
{
    /// <summary>
    /// Retrieves a booking by its Id with room and hotel details.
    /// </summary>
    /// <param name="id">The Id of the booking to search details for.</param>
    /// <returns>Bookings, null if not found.</returns>
    Task<Booking?> GetByIdWithDetailsAsync(int id);
    
    /// <summary>
    /// Retrieves all bookings made by a user.
    /// </summary>
    /// <param name="userId">The Id of the user to search for.</param>
    /// <returns>A collection of Bookings, null if not found.</returns>
    Task<IEnumerable<Booking>> GetByUserIdAsync(int userId);
}
