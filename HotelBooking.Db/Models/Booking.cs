using HotelBooking.Db.Enums;
using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Db.Models;

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

    /// <summary>
    /// Number of adults for the booking. Default = 1.
    /// </summary>
    [Range(1, 10)]
    public int NumberOfAdults { get; set; }

    /// <summary>
    /// Number of children for the booking. Default = 0.
    /// </summary>
    [Range(0, 10)]
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