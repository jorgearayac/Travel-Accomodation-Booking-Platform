namespace HotelBooking.Db.Models;

/// <summary>
/// Represents a city in the hotel booking system.
/// </summary>
public class City
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Country { get; set; }
    public required string PostOffice { get; set; }
    public string? ThumbnailUrl { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    // Navigation properties
    public ICollection<Hotel> Hotels { get; set; } = new List<Hotel>();
}