using HotelBooking.Db.Models;

namespace HotelBooking.Db.Interfaces;

public interface IHotelImageRepository : IRepository<HotelImage>
{
    // Get all images for a specific hotel
    Task<IEnumerable<HotelImage>> GetByHotelIdAsync(int hotelId);
}
