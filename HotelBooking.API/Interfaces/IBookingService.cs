using HotelBooking.API.Common;
using HotelBooking.API.DTOs.Bookings;
using HotelBooking.API.DTOs.Home;

namespace HotelBooking.API.Interfaces;

public interface IBookingService
{
    Task<Result<BookingResponse>> GetBookingByIdAsync(int id, int userId);
    Task<Result<IEnumerable<BookingResponse>>> GetBookingsByUserAsync(int userId);
    Task<Result<BookingResponse>> CreateBookingAsync(int userId, CreateBookingRequest request);
    Task<Result<IEnumerable<RecentlyBookedHotelResponse>>> GetRecentlyBookedHotelsAsync(int userId);
}
