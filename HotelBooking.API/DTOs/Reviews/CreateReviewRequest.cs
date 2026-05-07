using System.ComponentModel.DataAnnotations;

namespace HotelBooking.API.DTOs.Reviews;

public class CreateReviewRequest
{
    [Required]
    public int HotelId { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; }

    [StringLength(1000)]
    public string? Comment { get; set; }
}
