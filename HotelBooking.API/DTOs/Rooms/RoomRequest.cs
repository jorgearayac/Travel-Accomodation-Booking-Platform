using HotelBooking.Db.Enums;
using System.ComponentModel.DataAnnotations;

namespace HotelBooking.API.DTOs.Rooms;

public class RoomRequest
{
    [Required]
    public int HotelId { get; set; }

    [Required]
    [StringLength(20)]
    public required string RoomNumber { get; set; }
    public RoomType RoomType { get; set; }

    [Range(1, 10)]
    public int AdultCapacity { get; set; }

    [Range(0, 10)]
    public int ChildCapacity { get; set; }

    [Required]
    [StringLength(1000)]
    public required string Description { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal PricePerNight { get; set; }
    public bool Availability { get; set; }

    [Required]
    [StringLength(1000)]
    public required string ThumbnailUrl { get; set; }
}
