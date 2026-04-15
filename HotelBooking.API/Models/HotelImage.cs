namespace HotelBooking.API.Models;

/// <summary>
/// Represent an image associated with a hotel in the hotel booking system.
/// </summary>
public class HotelImage
{
    public int Id { get; set; }
    public int HotelId { get; set; }
    public required string ImageUrl { get; set; }
    public string? Caption { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    // Navigation properties
    public Hotel Hotel { get; set; } = null!;
}