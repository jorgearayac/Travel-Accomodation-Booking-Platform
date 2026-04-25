using HotelBooking.API.DTOs.Hotels;

namespace HotelBooking.API.Interfaces;

public interface IHotelImageService
{
    Task<IEnumerable<HotelImageResponse>> GetImagesByHotelIdAsync(int hotelId);
    Task<HotelImageResponse> CreateImageAsync(HotelImageRequest request);
    Task<HotelImageResponse> UpdateImageAsync(int id, HotelImageRequest request);
    Task DeleteImageAsync(int id);
}
