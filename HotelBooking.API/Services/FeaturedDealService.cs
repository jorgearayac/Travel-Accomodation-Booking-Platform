using HotelBooking.API.DTOs.FeaturedDeals;
using HotelBooking.API.Interfaces;
using HotelBooking.Db.Interfaces;
using HotelBooking.Db.Models;

namespace HotelBooking.API.Services;

public class FeaturedDealService : IFeaturedDealService
{
    private readonly IFeaturedDealRepository _featuredDealRepository;

    public FeaturedDealService(IFeaturedDealRepository featuredDealRepository)
    {
        _featuredDealRepository = featuredDealRepository;
    }

    public async Task<IEnumerable<FeaturedDealResponse>> GetAllFeaturedDealsAsync()
    {
        var deals = await _featuredDealRepository.GetAllWithHotelAndCityAsync();
        return deals.Take(5).Select(MapToResponse);
    }

    public async Task<FeaturedDealResponse?> GetFeaturedDealByIdAsync(int id)
    {
        var deal = await _featuredDealRepository.GetByIdWithHotelAndCityAsync(id);
        if (deal == null)
        {
            throw new KeyNotFoundException($"Featured deal with Id {id} not found.");
        }
        return MapToResponse(deal);
    }

    public async Task<FeaturedDealResponse> CreateFeaturedDealAsync(FeaturedDealRequest request)
    {
        var deal = new FeaturedDeal
        {
            HotelId = request.HotelId,
            OriginalPrice = request.OriginalPrice,
            DiscountedPrice = request.DiscountedPrice,
            Description = request.Description,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };
        await _featuredDealRepository.AddAsync(deal);

        // reload with hotel and city included
        var createdDeal = await _featuredDealRepository.GetByIdWithHotelAndCityAsync(deal.Id);
        return MapToResponse(createdDeal!);
    }

    public async Task<FeaturedDealResponse?> UpdateFeaturedDealAsync(int id, FeaturedDealRequest request)
    {
        var deal = await _featuredDealRepository.GetByIdAsync(id);
        if (deal == null)
        {
            throw new KeyNotFoundException($"Featured deal with Id {id} not found.");
        }
        deal.HotelId = request.HotelId;
        deal.OriginalPrice = request.OriginalPrice;
        deal.DiscountedPrice = request.DiscountedPrice;
        deal.Description = request.Description;
        deal.UpdatedDate = DateTime.UtcNow;

        await _featuredDealRepository.UpdateAsync(deal);

        // reload with hotel and city included
        var updatedDeal = await _featuredDealRepository.GetByIdWithHotelAndCityAsync(deal.Id);
        return MapToResponse(updatedDeal!);
    }

    public async Task DeleteFeaturedDealAsync(int id)
    {
        var deal = await _featuredDealRepository.GetByIdAsync(id);
        if (deal == null)
        {
            throw new KeyNotFoundException($"Featured deal with Id {id} not found.");
        }
        await _featuredDealRepository.DeleteAsync(deal);
    }

    // Helper method to map FeaturedDeal to FeaturedDealResponse
    private FeaturedDealResponse MapToResponse(FeaturedDeal deal)
    {
        return new FeaturedDealResponse
        {
            Id = deal.Id,
            HotelId = deal.HotelId,
            HotelName = deal.Hotel.Name,
            CityName = deal.Hotel.City.Name,
            StarRate = deal.Hotel.StarRate,
            OriginalPrice = deal.OriginalPrice,
            DiscountedPrice = deal.DiscountedPrice,
            Description = deal.Description,
            ThumbnailUrl = deal.Hotel.ThumbnailUrl
        };
    }
}