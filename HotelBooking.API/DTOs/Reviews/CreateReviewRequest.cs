using System.ComponentModel.DataAnnotations;

namespace HotelBooking.API.DTOs.Reviews;

public class CreateReviewRequest
{
    public int HotelId { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; }
    public string? Comment { get; set; }
}
