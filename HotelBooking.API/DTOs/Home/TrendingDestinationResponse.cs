namespace HotelBooking.API.DTOs.Home;

public class TrendingDestinationResponse
{
    public int CityId { get; set; }
    public required string CityName { get; set; }
    public required string Country { get; set; }
    public string? ThumbnailUrl { get; set; }
    public int BookingCount { get; set; }
}