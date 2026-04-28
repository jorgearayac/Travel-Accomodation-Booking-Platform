namespace HotelBooking.API.DTOs.Hotels;

public class HotelResponse
{
    public int Id { get; set; }
    public int CityId { get; set; }
    public required string Name { get; set; }
    public int StarRate { get; set; }
    public required string Owner { get; set; }
    public required string Description { get; set; }
    public decimal PricePerNight { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public required string ThumbnailUrl { get; set; }
    public int NumberOfRooms { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}