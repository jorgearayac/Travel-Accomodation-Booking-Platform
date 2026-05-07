using System.ComponentModel.DataAnnotations;

namespace HotelBooking.API.DTOs.Search;

public class HotelSearchRequest
{
    [StringLength(200)]
    public string? Query { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? MinPrice { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? MaxPrice { get; set; }

    [Range(1, 5)]
    public int? StarRate { get; set; }

    public string? RoomType { get; set; }

    public DateTime? CheckInDate { get; set; }
    public DateTime? CheckOutDate { get; set; }

    [Range(1, 20)]
    public int Adults { get; set; } = 2;

    [Range(0, 20)]
    public int Children { get; set; } = 0;

    [Range(1, 50)]
    public int Rooms { get; set; } = 1;

    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; } = 1;

    [Range(1, 100)]
    public int PageSize { get; set; } = 10;
}
