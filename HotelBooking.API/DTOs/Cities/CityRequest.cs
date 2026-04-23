namespace HotelBooking.API.DTOs.Cities;

public class CityRequest
{
    public required string Name { get; set; }
    public required string Country { get; set; }
    public required string PostOffice { get; set; }
    public string? ThumbnailUrl { get; set; }
}
