using HotelBooking.API.DTOs.Cities;
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

    public async Task<IEnumerable<CityResponse>> GetAllCitiesAsync()
    {
        var cities = await _cityRepository.GetAllWithHotelsAsync();
        return cities.Select(MapToResponse);
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

    // Helper method, refactor later
    private CityResponse MapToResponse(City city)
    {
        return new CityResponse
        {
            Id = city.Id,
            Name = city.Name,
            Country = city.Country,
            PostOffice = city.PostOffice,
            NumberOfHotels = city.Hotels?.Count ?? 0,
            CreatedDate = city.CreatedDate,
            UpdatedDate = city.UpdatedDate
        };
    }
}
