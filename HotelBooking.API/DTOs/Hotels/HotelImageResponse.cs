namespace HotelBooking.API.DTOs.Hotels;

public class HotelImageResponse
{
    public int Id { get; set; }
    public required string ImageUrl { get; set; }
    public string? Caption { get; set; }
}