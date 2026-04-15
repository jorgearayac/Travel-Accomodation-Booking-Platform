using HotelBooking.API.Enums;

namespace HotelBooking.API.Models;

/// <summary>
/// Represents a booking made by a user in the hotel booking system.
/// </summary>
public class Booking
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public required string ConfirmationNumber { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int NumberOfAdults { get; set; }
    public int NumberOfChildren { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string? SpecialRequests { get; set; }
    public decimal TotalPrice { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    // Navigation Properties
    public User User { get; set; } = null!;
    public ICollection<BookingRoom> BookingRooms { get; set; } = new List<BookingRoom>();
}