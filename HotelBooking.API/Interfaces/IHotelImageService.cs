using HotelBooking.API.Common;
using HotelBooking.API.DTOs.Hotels;

namespace HotelBooking.API.Interfaces;

public interface IHotelImageService
{
    Task<Result<IEnumerable<HotelImageResponse>>> GetImagesByHotelIdAsync(int hotelId);
    Task<Result<HotelImageResponse>> CreateImageAsync(HotelImageRequest request);
    Task<Result<HotelImageResponse>> UpdateImageAsync(int id, HotelImageRequest request);
    Task<Result> DeleteImageAsync(int id);
}
