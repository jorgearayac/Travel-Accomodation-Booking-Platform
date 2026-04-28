namespace HotelBooking.API.DTOs.FeaturedDeals;

public class FeaturedDealResponse
{
    public int Id { get; set; }
    public int HotelId { get; set; }
    public required string HotelName { get; set; }
    public required string CityName { get; set; }
    public int StarRate { get; set; }
    public decimal OriginalPrice { get; set; }
    public decimal DiscountedPrice { get; set; }
    public string? Description { get; set; }
    public required string ThumbnailUrl { get; set; }
}
