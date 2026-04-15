using HotelBooking.API.Enums;

namespace HotelBooking.API.Models;

/// <summary>
/// Represents a room inside a hotel in the hotel booking system.
/// </summary>
public class Room
{
    public int Id { get; set; }
    public int HotelId { get; set; }
    public required string RoomNumber { get; set; }
    public RoomType RoomType { get; set; }
    public int AdultCapacity { get; set; }
    public int ChildCapacity { get; set; }
    public required string Description { get; set; }
    public decimal PricePerNight { get; set; }
    public bool Availability { get; set; }
    public required string ThumbnailUrl { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    // Navigation properties
    public Hotel Hotel { get; set; } = null!;
    public ICollection<BookingRoom> BookingRooms { get; set; } = new List<BookingRoom>();
}