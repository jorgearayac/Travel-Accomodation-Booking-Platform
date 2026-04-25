namespace HotelBooking.API.DTOs.Hotels;

public class HotelImageRequest
{
    public int HotelId { get; set; }
    public required string ImageUrl { get; set; }
    public string? Caption { get; set; }
}