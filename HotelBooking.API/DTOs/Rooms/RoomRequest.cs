using HotelBooking.Db.Enums;
using System.ComponentModel.DataAnnotations;

namespace HotelBooking.API.DTOs.Rooms;

public class RoomRequest
{
    public int HotelId { get; set; }
    public required string RoomNumber { get; set; }
    public RoomType RoomType { get; set; }

    [Range(1, 10)]
    public int AdultCapacity { get; set; }

    [Range(0, 10)]
    public int ChildCapacity { get; set; }
    public required string Description { get; set; }
    public decimal PricePerNight { get; set; }
    public bool Availability { get; set; }
    public required string ThumbnailUrl { get; set; }
}