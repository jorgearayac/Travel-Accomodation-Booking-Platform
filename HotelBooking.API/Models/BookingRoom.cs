namespace HotelBooking.API.Models;

public class BookingRoom
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public int RoomId { get; set; }
    public decimal PriceAtBooking { get; set; }

    // Navigation properties
    public Booking Booking { get; set; } = null!;
    public Room Room { get; set; } = null!;
}