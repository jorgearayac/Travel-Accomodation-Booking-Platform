namespace HotelBooking.API.Models;

public class FeaturedDeal
{
    public int Id { get; set; }
    public int HotelId { get; set; }
    public decimal OriginalPrice { get; set; }
    public decimal DiscountedPrice { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    // Navigation properties: To be implemented
}