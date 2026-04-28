using HotelBooking.Db.Enums;
using System.ComponentModel.DataAnnotations;

namespace HotelBooking.API.DTOs.Bookings;

public class CreateBookingRequest
{
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }

    [Range(1, 10)]
    public int NumberOfAdults { get; set; }

    [Range(0, 10)]
    public int NumberOfChildren { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string? SpecialRequests { get; set; }
    public required List<int> RoomIds { get; set; }
}