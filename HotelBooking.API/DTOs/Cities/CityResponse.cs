namespace HotelBooking.API.DTOs.Cities;

public class CityResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Country { get; set; }
    public required string PostOffice { get; set; }
    public string? ThumbnailUrl { get; set; }
    public int NumberOfHotels { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}