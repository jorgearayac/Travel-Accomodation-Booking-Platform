using HotelBooking.API.Common;
using HotelBooking.API.DTOs.FeaturedDeals;

namespace HotelBooking.API.Interfaces;

public interface IFeaturedDealService
{
    Task<Result<IEnumerable<FeaturedDealResponse>>> GetAllFeaturedDealsAsync();
    Task<Result<FeaturedDealResponse>> GetFeaturedDealByIdAsync(int id);
    Task<Result<FeaturedDealResponse>> CreateFeaturedDealAsync(FeaturedDealRequest request);
    Task<Result<FeaturedDealResponse>> UpdateFeaturedDealAsync(int id, FeaturedDealRequest request);
    Task<Result> DeleteFeaturedDealAsync(int id);
}
