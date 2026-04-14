namespace HotelBooking.API.Models;

public class City
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Country { get; set; }
    public required string PostOffice { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    // Navigation properties
    public ICollection<Hotel> Hotels { get; set; } = new List<Hotel>();
}