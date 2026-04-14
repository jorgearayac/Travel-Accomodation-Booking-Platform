using HotelBooking.API.Enums;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Models;

/// <summary>
/// Represents a user in the hotel booking system.
/// </summary>
public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    /// <summary>
    /// Hashed password for the user. Never store plain text passwords.
    /// </summary>
    public required string PasswordHash { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public UserRole Role { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    // Navigation properties
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<VisitedHotel> VisitedHotels { get; set; } = new List<VisitedHotel>();
}
