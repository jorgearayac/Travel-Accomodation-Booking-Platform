using HotelBooking.API.Common;
using HotelBooking.API.DTOs.FeaturedDeals;
using HotelBooking.API.Interfaces;
using HotelBooking.Db.Interfaces;
using HotelBooking.Db.Models;

namespace HotelBooking.API.Services;

public class FeaturedDealService : IFeaturedDealService
{
    private readonly IFeaturedDealRepository _featuredDealRepository;
    private readonly IHotelRepository _hotelRepository;
    private readonly ILogger<FeaturedDealService> _logger;

    public FeaturedDealService(
        IFeaturedDealRepository featuredDealRepository,
        IHotelRepository hotelRepository,
        ILogger<FeaturedDealService> logger)
    {
        _featuredDealRepository = featuredDealRepository;
        _hotelRepository = hotelRepository;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<FeaturedDealResponse>>> GetAllFeaturedDealsAsync()
    {
        var deals = await _featuredDealRepository.GetAllWithHotelAndCityAsync();
        return Result<IEnumerable<FeaturedDealResponse>>.Success(deals.Take(5).Select(MapToResponse));
    }

    public async Task<Result<FeaturedDealResponse>> GetFeaturedDealByIdAsync(int id)
    {
        var deal = await _featuredDealRepository.GetByIdWithHotelAndCityAsync(id);
        if (deal == null)
        {
            return Result<FeaturedDealResponse>.NotFound($"Featured deal with Id {id} not found.");
        }
        return Result<FeaturedDealResponse>.Success(MapToResponse(deal));
    }

    public async Task<Result<FeaturedDealResponse>> CreateFeaturedDealAsync(FeaturedDealRequest request)
    {
        var hotel = await _hotelRepository.GetByIdAsync(request.HotelId);
        if (hotel == null)
        {
            return Result<FeaturedDealResponse>.NotFound($"Hotel with Id {request.HotelId} not found.");
        }

        if (request.DiscountedPrice >= request.OriginalPrice)
        {
            return Result<FeaturedDealResponse>.ValidationError("Discounted price must be less than the original price.");
        }

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

        _logger.LogInformation("Featured deal created for hotel {HotelId}", request.HotelId);

        var createdDeal = await _featuredDealRepository.GetByIdWithHotelAndCityAsync(deal.Id);
        return Result<FeaturedDealResponse>.Success(MapToResponse(createdDeal!));
    }

    public async Task<Result<FeaturedDealResponse>> UpdateFeaturedDealAsync(int id, FeaturedDealRequest request)
    {
        var deal = await _featuredDealRepository.GetByIdAsync(id);
        if (deal == null)
        {
            return Result<FeaturedDealResponse>.NotFound($"Featured deal with Id {id} not found.");
        }

        if (request.DiscountedPrice >= request.OriginalPrice)
        {
            return Result<FeaturedDealResponse>.ValidationError("Discounted price must be less than the original price.");
        }

        deal.HotelId = request.HotelId;
        deal.OriginalPrice = request.OriginalPrice;
        deal.DiscountedPrice = request.DiscountedPrice;
        deal.Description = request.Description;
        deal.UpdatedDate = DateTime.UtcNow;

        await _featuredDealRepository.UpdateAsync(deal);

        var updatedDeal = await _featuredDealRepository.GetByIdWithHotelAndCityAsync(deal.Id);
        return Result<FeaturedDealResponse>.Success(MapToResponse(updatedDeal!));
    }

    public async Task<Result> DeleteFeaturedDealAsync(int id)
    {
        var deal = await _featuredDealRepository.GetByIdAsync(id);
        if (deal == null)
        {
            return Result.NotFound($"Featured deal with Id {id} not found.");
        }
        await _featuredDealRepository.DeleteAsync(deal);
        _logger.LogInformation("Featured deal {DealId} deleted", id);
        return Result.Success();
    }

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
