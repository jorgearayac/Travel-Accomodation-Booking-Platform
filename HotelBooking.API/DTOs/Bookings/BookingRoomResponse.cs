namespace HotelBooking.API.DTOs.Bookings;

public class BookingRoomResponse
{
    public int RoomId { get; set; }
    public required string RoomNumber { get; set; }
    public required string HotelName { get; set; }
    public required string RoomType { get; set; }
    public decimal PriceAtBooking { get; set; }
}