using HotelBooking.API.Common;
using HotelBooking.API.DTOs.Hotels;
using HotelBooking.API.DTOs.Pagination;
using HotelBooking.API.DTOs.Search;

namespace HotelBooking.API.Interfaces;

public interface IHotelService
{
    Task<Result<PaginationResponse<HotelResponse>>> GetAllHotelsAsync(PaginationRequest pagination);
    Task<Result<HotelResponse>> GetHotelByIdAsync(int id);
    Task<Result<HotelResponse>> CreateHotelAsync(HotelRequest request);
    Task<Result<HotelResponse>> UpdateHotelAsync(int id, HotelRequest request);
    Task<Result> DeleteHotelAsync(int id);
    Task<Result<PaginationResponse<HotelResponse>>> SearchHotelsAsync(HotelSearchRequest request);
    Task<Result<HotelDetailsResponse>> GetHotelDetailsAsync(int id);
}
