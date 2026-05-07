using HotelBooking.API.DTOs.Bookings;
using HotelBooking.API.DTOs.Home;

namespace HotelBooking.API.Interfaces;

/// <summary>
/// Interface for booking-related operations in the system.
/// </summary>
public interface IBookingService
{
    /// <summary>
    /// Retrieves a booking by its Id, verifying it belongs to the specified user.
    /// </summary>
    /// <param name="id">The Id of the booking to search for.</param>
    /// <param name="userId">The Id of the requesting user.</param>
    /// <returns>BookingResponse if found and owned by the user.</returns>
    Task<BookingResponse> GetBookingByIdAsync(int id, int userId);

    /// <summary>
    /// Retrieves all bookings made by a user.
    /// </summary>
    /// <param name="userId">The Id of the user to search bookings for.</param>
    /// <returns>A collection of BookingResponse if found, null otherwise.</returns>
    Task<IEnumerable<BookingResponse>> GetBookingsByUserAsync(int userId);

    /// <summary>
    /// Creates a booking for a user.
    /// </summary>
    /// <param name="userId">The Id of the user who is creating the booking.</param>
    /// <param name="request">The booking details.</param>
    /// <returns>BookingResponse if found, null otherwise.</returns>
    Task<BookingResponse> CreateBookingAsync(int userId, CreateBookingRequest request);

    /// <summary>
    /// Retrieves recently booked hotels made by a user.
    /// </summary>
    /// <param name="userId">The Id of the user to search the recently booked hotels for.</param>
    /// <returns>A collection of BookingResponse, null otherwise.</returns>
    Task<IEnumerable<RecentlyBookedHotelResponse>> GetRecentlyBookedHotelsAsync(int userId);
}