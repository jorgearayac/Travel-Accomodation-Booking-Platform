namespace HotelBooking.API.DTOs.Search;

public class HotelSearchRequest
{
    public string? Query { get; set; }

    // Filters for hotel search
    // Price filter
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }

    // Star rating filter
    public int? StarRate { get; set; }

    // Room type filter
    public string? RoomType { get; set; }

    // Date range filter for availability
    public DateTime? CheckInDate { get; set; }
    public DateTime? CheckOutDate { get; set; }

    // Default values for search parameters
    public int Adults { get; set; } = 2;
    public int Children { get; set; } = 0;
    public int Rooms { get; set; } = 1;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;

}
