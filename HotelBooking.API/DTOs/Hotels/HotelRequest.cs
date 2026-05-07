using System.ComponentModel.DataAnnotations;

namespace HotelBooking.API.DTOs.Hotels;

public class HotelRequest
{
    [Required]
    public int CityId { get; set; }

    [Required]
    [StringLength(200)]
    public required string Name { get; set; }

    [Range(1, 5)]
    public int StarRate { get; set; }

    [Required]
    [StringLength(100)]
    public required string Owner { get; set; }

    [Required]
    [StringLength(1000)]
    public required string Description { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal PricePerNight { get; set; }

    [Range(-90, 90)]
    public double? Latitude { get; set; }

    [Range(-180, 180)]
    public double? Longitude { get; set; }

    [Required]
    [StringLength(1000)]
    public required string ThumbnailUrl { get; set; }
}
