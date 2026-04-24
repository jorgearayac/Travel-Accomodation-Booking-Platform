namespace HotelBooking.API.DTOs.Reviews;

public class ReviewResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public required string Username { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedDate { get; set; }
}