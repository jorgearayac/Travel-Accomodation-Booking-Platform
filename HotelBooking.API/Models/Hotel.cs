namespace HotelBooking.API.Models;

/// <summary>
/// Represents a hotel in the hotel booking system.
/// </summary>
public class Hotel
{
    public int Id { get; set; }
    public int CityId { get; set; }
    public required string Name { get; set; }
    public int StarRate { get; set; } // numbers from 1 to 5
    public required string Owner { get; set; }
    public required string Description { get; set; }
    public decimal PricePerNight { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public required string ThumbnailUrl { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    // Navigation properties
    public City City { get; set; } = null!;
    public ICollection<Room> Rooms { get; set; } = new List<Room>();
    public ICollection<HotelImage> HotelImages { get; set; } = new List<HotelImage>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<FeaturedDeal> FeaturedDeals { get; set; } = new List<FeaturedDeal>();
    public ICollection<VisitedHotel> VisitedHotels { get; set; } = new List<VisitedHotel>();
}
