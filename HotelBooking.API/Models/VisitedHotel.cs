namespace HotelBooking.API.Models;

public class VisitedHotel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int HotelId { get; set; }
    public DateTime VisitDate { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public Hotel Hotel { get; set; } = null!;
}