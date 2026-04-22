using HotelBooking.API.DTOs.Bookings;
using HotelBooking.API.DTOs.Rooms;

namespace HotelBooking.API.Interfaces;

public interface IBookingService
{
    Task<BookingResponse> GetBookingByIdAsync(int id);
    Task<IEnumerable<BookingResponse>> GetBookingsByUserAsync(int userId);
    Task<BookingResponse> CreateBookingAsync(int userId, CreateBookingRequest request);
}