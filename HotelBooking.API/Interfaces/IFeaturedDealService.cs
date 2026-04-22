using HotelBooking.API.DTOs.FeaturedDeals;

namespace HotelBooking.API.Interfaces;

public interface IFeaturedDealService
{
    Task<IEnumerable<FeaturedDealResponse>> GetAllFeaturedDealsAsync();
    Task<FeaturedDealResponse?> GetFeaturedDealByIdAsync(int id);
    Task<FeaturedDealResponse> CreateFeaturedDealAsync(FeaturedDealRequest request);
    Task<FeaturedDealResponse?> UpdateFeaturedDealAsync(int id, FeaturedDealRequest request);
    Task DeleteFeaturedDealAsync(int id);
}
