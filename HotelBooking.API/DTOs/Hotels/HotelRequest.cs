using System.ComponentModel.DataAnnotations;

namespace HotelBooking.API.DTOs.Hotels;

/// <summary>
/// Format for the requests to create or update a hotel in the system.
/// </summary>
public class HotelRequest
{
    public int CityId { get; set; }
    public required string Name { get; set; }
    public int StarRate { get; set; }
    public required string Owner { get; set; }
    public required string Description { get; set; }
    public decimal PricePerNight { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public required string ThumbnailUrl { get; set; }
}
