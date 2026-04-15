namespace HotelBooking.API.Models;

/// <summary>
/// Represents a featured deal for a hotel in the hotel booking system. This entity is used to highlight special offers.
/// </summary>
public class FeaturedDeal
{
    public int Id { get; set; }
    public int HotelId { get; set; }
    public decimal OriginalPrice { get; set; }
    public decimal DiscountedPrice { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    // Navigation properties
    public Hotel Hotel { get; set; } = null!;
}