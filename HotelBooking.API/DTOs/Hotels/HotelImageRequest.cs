using System.ComponentModel.DataAnnotations;

namespace HotelBooking.API.DTOs.Hotels;

public class HotelImageRequest
{
    public int HotelId { get; set; }

    [Required]
    [StringLength(1000)]
    public required string ImageUrl { get; set; }

    [StringLength(500)]
    public string? Caption { get; set; }
}
