using HotelBooking.API.Common;
using HotelBooking.API.DTOs.Cities;
using HotelBooking.API.DTOs.Home;
using HotelBooking.API.DTOs.Pagination;

namespace HotelBooking.API.Interfaces;

public interface ICityService
{
    Task<Result<PaginationResponse<CityResponse>>> GetAllCitiesAsync(PaginationRequest pagination);
    Task<Result<CityResponse>> GetCityByIdAsync(int id);
    Task<Result<CityResponse>> CreateCityAsync(CityRequest request);
    Task<Result<CityResponse>> UpdateCityAsync(int id, CityRequest request);
    Task<Result> DeleteCityAsync(int id);
    Task<Result<IEnumerable<TrendingDestinationResponse>>> GetTrendingDestinationsAsync();
}
