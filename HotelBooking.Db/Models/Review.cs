using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Db.Models;

/// <summary>
/// Represents a review left by a user for a hotel in the hotel booking system.
/// </summary>
public class Review
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int HotelId { get; set; }

    /// <summary>
    /// Ratings from 1 to 5 for a hotel review.
    /// </summary>
    [Range(1, 5)]
    public int Rating { get; set; } // numbers between 1 and 5
    public string? Comment { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public Hotel Hotel { get; set; } = null!;
}