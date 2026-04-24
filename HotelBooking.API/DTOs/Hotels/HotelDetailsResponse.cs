using HotelBooking.API.DTOs.Reviews;
using HotelBooking.API.DTOs.Rooms;

namespace HotelBooking.API.DTOs.Hotels;

public class HotelDetailsResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int StarRate { get; set; }
    public required string Owner { get; set; }
    public required string Description { get; set; }
    public decimal PricePerNight { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public required string ThumbnailUrl { get; set; }
    public required string CityName { get; set; }
    public List<HotelImageResponse> Images { get; set; } = new();
    public List<ReviewResponse> Reviews { get; set; } = new();
    public List<RoomResponse> AvailableRooms { get; set; } = new();
}
