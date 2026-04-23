public class RecentlyBookedHotelResponse
{
    public int HotelId { get; set; }
    public required string HotelName { get; set; }
    public required string CityName { get; set; }
    public int StarRate { get; set; }
    public decimal PricePerNight { get; set; }
    public required string ThumbnailUrl { get; set; }
    public DateTime BookingDate { get; set; }
}
