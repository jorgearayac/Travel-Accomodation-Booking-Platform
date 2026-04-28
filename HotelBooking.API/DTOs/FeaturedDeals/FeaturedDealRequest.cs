namespace HotelBooking.API.DTOs.FeaturedDeals;

public class FeaturedDealRequest
{
    public int HotelId { get; set; }
    public decimal OriginalPrice { get; set; }
    public decimal DiscountedPrice { get; set; }
    public required string Description { get; set; }
}