using HotelBooking.Db.Enums;
using System.ComponentModel.DataAnnotations;

namespace HotelBooking.API.DTOs.Bookings;

public class CreateBookingRequest
{
    [Required]
    public DateTime CheckInDate { get; set; }

    [Required]
    public DateTime CheckOutDate { get; set; }

    [Range(1, 10)]
    public int NumberOfAdults { get; set; }

    [Range(0, 10)]
    public int NumberOfChildren { get; set; }
    public PaymentMethod PaymentMethod { get; set; }

    [StringLength(1000)]
    public string? SpecialRequests { get; set; }

    [Required]
    [MinLength(1)]
    public required List<int> RoomIds { get; set; }
}
