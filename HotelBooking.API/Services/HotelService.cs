using HotelBooking.API.Common;
using HotelBooking.API.DTOs.Hotels;
using HotelBooking.API.DTOs.Pagination;
using HotelBooking.API.DTOs.Reviews;
using HotelBooking.API.DTOs.Rooms;
using HotelBooking.API.DTOs.Search;
using HotelBooking.API.Interfaces;
using HotelBooking.Db.Interfaces;
using HotelBooking.Db.Models;

namespace HotelBooking.API.Services;

public class HotelService : IHotelService
{
    private readonly IHotelRepository _hotelRepository;
    private readonly ICityRepository _cityRepository;
    private readonly ILogger<HotelService> _logger;

    public HotelService(
        IHotelRepository hotelRepository,
        ICityRepository cityRepository,
        ILogger<HotelService> logger)
    {
        _hotelRepository = hotelRepository;
        _cityRepository = cityRepository;
        _logger = logger;
    }

    public async Task<Result<PaginationResponse<HotelResponse>>> GetAllHotelsAsync(PaginationRequest pagination)
    {
        var (hotels, totalCount) = await _hotelRepository.GetPaginatedWithRoomsAsync(pagination.PageNumber, pagination.PageSize);
        var totalPages = (int)Math.Ceiling(totalCount / (double)pagination.PageSize);

        return Result<PaginationResponse<HotelResponse>>.Success(new PaginationResponse<HotelResponse>
        {
            Items = hotels.Select(MapToResponse),
            TotalCount = totalCount,
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalPages = totalPages,
            HasPreviousPage = pagination.PageNumber > 1,
            HasNextPage = pagination.PageNumber < totalPages
        });
    }

    public async Task<Result<HotelResponse>> GetHotelByIdAsync(int id)
    {
        var hotel = await _hotelRepository.GetByIdWithRoomsAsync(id);
        if (hotel == null)
        {
            return Result<HotelResponse>.NotFound($"Hotel with Id {id} not found.");
        }
        return Result<HotelResponse>.Success(MapToResponse(hotel));
    }

    public async Task<Result<HotelResponse>> CreateHotelAsync(HotelRequest request)
    {
        var city = await _cityRepository.GetByIdAsync(request.CityId);
        if (city == null)
        {
            return Result<HotelResponse>.NotFound($"City with Id {request.CityId} not found.");
        }

        var hotel = new Hotel
        {
            CityId = request.CityId,
            Name = request.Name,
            StarRate = request.StarRate,
            Owner = request.Owner,
            Description = request.Description,
            PricePerNight = request.PricePerNight,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            ThumbnailUrl = request.ThumbnailUrl,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };
        await _hotelRepository.AddAsync(hotel);
        _logger.LogInformation("Hotel {HotelName} created in city {CityId}", request.Name, request.CityId);
        return Result<HotelResponse>.Success(MapToResponse(hotel));
    }

    public async Task<Result<HotelResponse>> UpdateHotelAsync(int id, HotelRequest request)
    {
        var hotel = await _hotelRepository.GetByIdAsync(id);
        if (hotel == null)
        {
            return Result<HotelResponse>.NotFound($"Hotel with Id {id} not found.");
        }
        hotel.CityId = request.CityId;
        hotel.Name = request.Name;
        hotel.StarRate = request.StarRate;
        hotel.Owner = request.Owner;
        hotel.Description = request.Description;
        hotel.PricePerNight = request.PricePerNight;
        hotel.Latitude = request.Latitude;
        hotel.Longitude = request.Longitude;
        hotel.ThumbnailUrl = request.ThumbnailUrl;
        hotel.UpdatedDate = DateTime.UtcNow;

        await _hotelRepository.UpdateAsync(hotel);
        return Result<HotelResponse>.Success(MapToResponse(hotel));
    }

    public async Task<Result> DeleteHotelAsync(int id)
    {
        var hotel = await _hotelRepository.GetByIdAsync(id);
        if (hotel == null)
        {
            return Result.NotFound($"Hotel with Id {id} not found.");
        }
        await _hotelRepository.DeleteAsync(hotel);
        _logger.LogInformation("Hotel {HotelId} deleted", id);
        return Result.Success();
    }

    public async Task<Result<PaginationResponse<HotelResponse>>> SearchHotelsAsync(HotelSearchRequest request)
    {
        var (hotels, totalCount) = await _hotelRepository.SearchAsync(
            request.Query, request.MinPrice, request.MaxPrice, request.StarRate,
            request.RoomType, request.Adults, request.Children,
            request.PageNumber, request.PageSize);

        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        return Result<PaginationResponse<HotelResponse>>.Success(new PaginationResponse<HotelResponse>
        {
            Items = hotels.Select(MapToResponse),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalPages = totalPages,
            HasPreviousPage = request.PageNumber > 1,
            HasNextPage = request.PageNumber < totalPages
        });
    }

    public async Task<Result<HotelDetailsResponse>> GetHotelDetailsAsync(int id)
    {
        var hotel = await _hotelRepository.GetByIdWithFullDetailsAsync(id);
        if (hotel == null)
        {
            return Result<HotelDetailsResponse>.NotFound($"Hotel with Id {id} not found.");
        }
        return Result<HotelDetailsResponse>.Success(MapToDetailsResponse(hotel));
    }

    private HotelResponse MapToResponse(Hotel hotel)
    {
        return new HotelResponse
        {
            Id = hotel.Id,
            Name = hotel.Name,
            StarRate = hotel.StarRate,
            Owner = hotel.Owner,
            Description = hotel.Description,
            PricePerNight = hotel.PricePerNight,
            Latitude = hotel.Latitude,
            Longitude = hotel.Longitude,
            ThumbnailUrl = hotel.ThumbnailUrl,
            NumberOfRooms = hotel.Rooms?.Count ?? 0,
            CreatedDate = hotel.CreatedDate,
            UpdatedDate = hotel.UpdatedDate
        };
    }

    private HotelDetailsResponse MapToDetailsResponse(Hotel hotel)
    {
        return new HotelDetailsResponse
        {
            Id = hotel.Id,
            Name = hotel.Name,
            StarRate = hotel.StarRate,
            Owner = hotel.Owner,
            Description = hotel.Description,
            PricePerNight = hotel.PricePerNight,
            Latitude = hotel.Latitude,
            Longitude = hotel.Longitude,
            ThumbnailUrl = hotel.ThumbnailUrl,
            CityName = hotel.City?.Name ?? string.Empty,
            Images = hotel.HotelImages?.Select(hi => new HotelImageResponse
            {
                Id = hi.Id, ImageUrl = hi.ImageUrl, Caption = hi.Caption
            }).ToList() ?? new List<HotelImageResponse>(),
            Reviews = hotel.Reviews?.Select(r => new ReviewResponse
            {
                Id = r.Id, UserId = r.UserId,
                Username = r.User?.Username ?? "Unknown User",
                HotelId = r.HotelId,
                HotelName = r.Hotel?.Name ?? "Unknown Hotel",
                Rating = r.Rating, Comment = r.Comment, CreatedDate = r.CreatedDate
            }).ToList() ?? new List<ReviewResponse>(),
            AvailableRooms = hotel.Rooms?.Where(r => r.Availability).Select(r => new RoomResponse
            {
                Id = r.Id, RoomNumber = r.RoomNumber, RoomType = r.RoomType,
                AdultCapacity = r.AdultCapacity, ChildCapacity = r.ChildCapacity,
                Description = r.Description, PricePerNight = r.PricePerNight,
                ThumbnailUrl = r.ThumbnailUrl
            }).ToList() ?? new List<RoomResponse>()
        };
    }
}
