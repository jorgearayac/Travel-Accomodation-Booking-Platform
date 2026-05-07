using HotelBooking.API.Common;
using HotelBooking.API.DTOs.Cities;
using HotelBooking.API.DTOs.Home;
using HotelBooking.API.DTOs.Pagination;
using HotelBooking.API.Interfaces;
using HotelBooking.Db.Interfaces;
using HotelBooking.Db.Models;

namespace HotelBooking.API.Services;

public class CityService : ICityService
{
    private readonly ICityRepository _cityRepository;
    private readonly ILogger<CityService> _logger;

    public CityService(ICityRepository cityRepository, ILogger<CityService> logger)
    {
        _cityRepository = cityRepository;
        _logger = logger;
    }

    public async Task<Result<PaginationResponse<CityResponse>>> GetAllCitiesAsync(PaginationRequest pagination)
    {
        var (cities, totalCount) = await _cityRepository.GetPaginatedWithHotelsAsync(pagination.PageNumber, pagination.PageSize);
        var totalPages = (int)Math.Ceiling(totalCount / (double)pagination.PageSize);

        return Result<PaginationResponse<CityResponse>>.Success(new PaginationResponse<CityResponse>
        {
            Items = cities.Select(MapToResponse),
            TotalCount = totalCount,
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalPages = totalPages,
            HasPreviousPage = pagination.PageNumber > 1,
            HasNextPage = pagination.PageNumber < totalPages
        });
    }

    public async Task<Result<CityResponse>> GetCityByIdAsync(int id)
    {
        var city = await _cityRepository.GetByIdWithHotelsAsync(id);
        if (city == null)
        {
            return Result<CityResponse>.NotFound($"City with Id {id} not found.");
        }
        return Result<CityResponse>.Success(MapToResponse(city));
    }

    public async Task<Result<CityResponse>> CreateCityAsync(CityRequest request)
    {
        var city = new City
        {
            Name = request.Name,
            Country = request.Country,
            PostOffice = request.PostOffice,
            ThumbnailUrl = request.ThumbnailUrl,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };
        await _cityRepository.AddAsync(city);
        _logger.LogInformation("City {CityName} created", request.Name);
        return Result<CityResponse>.Success(MapToResponse(city));
    }

    public async Task<Result<CityResponse>> UpdateCityAsync(int id, CityRequest request)
    {
        var city = await _cityRepository.GetByIdAsync(id);
        if (city == null)
        {
            return Result<CityResponse>.NotFound($"City with Id {id} not found.");
        }
        city.Name = request.Name;
        city.Country = request.Country;
        city.PostOffice = request.PostOffice;
        city.ThumbnailUrl = request.ThumbnailUrl;
        city.UpdatedDate = DateTime.UtcNow;

        await _cityRepository.UpdateAsync(city);
        return Result<CityResponse>.Success(MapToResponse(city));
    }

    public async Task<Result> DeleteCityAsync(int id)
    {
        var city = await _cityRepository.GetByIdAsync(id);
        if (city == null)
        {
            return Result.NotFound($"City with Id {id} not found.");
        }
        await _cityRepository.DeleteAsync(city);
        _logger.LogInformation("City {CityId} deleted", id);
        return Result.Success();
    }

    public async Task<Result<IEnumerable<TrendingDestinationResponse>>> GetTrendingDestinationsAsync()
    {
        var results = await _cityRepository.GetTopBookedCitiesAsync(5);
        var destinations = results.Select(r => new TrendingDestinationResponse
        {
            CityId = r.City.Id,
            CityName = r.City.Name,
            Country = r.City.Country,
            ThumbnailUrl = r.City.ThumbnailUrl ?? "",
            BookingCount = r.BookingCount
        });

        return Result<IEnumerable<TrendingDestinationResponse>>.Success(destinations);
    }

    private CityResponse MapToResponse(City city)
    {
        return new CityResponse
        {
            Id = city.Id,
            Name = city.Name,
            Country = city.Country,
            PostOffice = city.PostOffice,
            ThumbnailUrl = city.ThumbnailUrl,
            NumberOfHotels = city.Hotels?.Count ?? 0,
            CreatedDate = city.CreatedDate,
            UpdatedDate = city.UpdatedDate
        };
    }
}
