using System.ComponentModel.DataAnnotations;

namespace HotelBooking.API.DTOs.Cities;

public class CityRequest
{
    [Required]
    [StringLength(200)]
    public required string Name { get; set; }

    [Required]
    [StringLength(200)]
    public required string Country { get; set; }

    [Required]
    [StringLength(50)]
    public required string PostOffice { get; set; }

    [StringLength(1000)]
    public string? ThumbnailUrl { get; set; }
}
