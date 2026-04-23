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

    public CityService(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }

    public async Task<PaginationResponse<CityResponse>> GetAllCitiesAsync(PaginationRequest pagination)
    {
        var (cities, totalCount) = await _cityRepository.GetPaginatedWithHotelsAsync(pagination.PageNumber, pagination.PageSize);
        var totalPages = (int)Math.Ceiling(totalCount / (double)pagination.PageSize); // Math.Ceiling to round up to the nearest whole number

        return new PaginationResponse<CityResponse>
        {
            Items = cities.Select(MapToResponse),
            TotalCount = totalCount,
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalPages = totalPages,
            HasPreviousPage = pagination.PageNumber > 1,
            HasNextPage = pagination.PageNumber < totalPages
        };
    }

    public async Task<CityResponse> GetCityByIdAsync(int id)
    {
        var city = await _cityRepository.GetByIdWithHotelsAsync(id);
        if (city == null)
        {
            throw new KeyNotFoundException($"City with Id {id} not found.");
        }
        return MapToResponse(city);
    }

    public async Task<CityResponse> CreateCityAsync(CityRequest request)
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
        return MapToResponse(city);
    }

    public async Task<CityResponse> UpdateCityAsync(int id, CityRequest request)
    {
        var city = await _cityRepository.GetByIdAsync(id);
        if (city == null)
        {
            throw new KeyNotFoundException($"City with Id {id} not found.");
        }
        city.Name = request.Name;
        city.Country = request.Country;
        city.PostOffice = request.PostOffice;
        city.ThumbnailUrl = request.ThumbnailUrl;
        city.UpdatedDate = DateTime.UtcNow;

        await _cityRepository.UpdateAsync(city);
        return MapToResponse(city);
    }
    
    public async Task DeleteCityAsync(int id)
    {
        var city = await _cityRepository.GetByIdAsync(id);
        if (city == null)
        {
            throw new KeyNotFoundException($"City with Id {id} not found.");
        }
        await _cityRepository.DeleteAsync(city);
    }

    public async Task<IEnumerable<TrendingDestinationResponse>> GetTrendingDestinationsAsync()
    {
        var cities = await _cityRepository.GetTopBookedCitiesAsync(5);
        return cities.Select(c => new TrendingDestinationResponse
        {
            CityId = c.Id,
            CityName = c.Name,
            Country = c.Country,
            ThumbnailUrl = c.ThumbnailUrl ?? "", // "" for already created cities without thumbnail
            BookingCount = c.Hotels
                .SelectMany(h => h.Rooms)
                .SelectMany(r => r.BookingRooms)
                .Count()
        });
    }

    // Helper method, refactor later
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
