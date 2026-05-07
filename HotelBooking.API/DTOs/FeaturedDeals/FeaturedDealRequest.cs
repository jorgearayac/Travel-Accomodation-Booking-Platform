using System.ComponentModel.DataAnnotations;

namespace HotelBooking.API.DTOs.FeaturedDeals;

public class FeaturedDealRequest
{
    [Required]
    public int HotelId { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal OriginalPrice { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal DiscountedPrice { get; set; }

    [Required]
    [StringLength(200)]
    public required string Description { get; set; }
}
