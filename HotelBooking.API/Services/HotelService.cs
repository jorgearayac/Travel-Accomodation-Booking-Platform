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

    public HotelService(IHotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }
    public async Task<PaginationResponse<HotelResponse>> GetAllHotelsAsync(PaginationRequest pagination)
    {
        var (hotels, totalCount) = await _hotelRepository.GetPaginatedWithRoomsAsync(pagination.PageNumber, pagination.PageSize);
        var totalPages = (int)Math.Ceiling(totalCount / (double)pagination.PageSize); // Math.Ceiling to round up to the nearest whole number

        return new PaginationResponse<HotelResponse>
        {
            Items = hotels.Select(MapToResponse),
            TotalCount = totalCount,
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalPages = totalPages,
            HasPreviousPage = pagination.PageNumber > 1,
            HasNextPage = pagination.PageNumber < totalPages
        };
    }

    public async Task<HotelResponse> GetHotelByIdAsync(int id)
    {
        var hotel = await _hotelRepository.GetByIdWithRoomsAsync(id);
        if (hotel == null)
        {
            throw new KeyNotFoundException($"Hotel with Id {id} not found.");
        }
        return MapToResponse(hotel);
    }

    public async Task<HotelResponse> CreateHotelAsync(HotelRequest request)
    {
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
        return MapToResponse(hotel);
    }

    public async Task<HotelResponse> UpdateHotelAsync(int id, HotelRequest request)
    {
        var hotel = await _hotelRepository.GetByIdAsync(id);
        if (hotel == null)
        {
            throw new KeyNotFoundException($"Hotel with Id {id} not found.");
        }
        hotel.CityId = request.CityId; // may refactor later
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
        return MapToResponse(hotel);
    }
    public async Task DeleteHotelAsync(int id)
    {
        var hotel = await _hotelRepository.GetByIdAsync(id);
        if (hotel == null)
        {
            throw new KeyNotFoundException($"Hotel with Id {id} not found.");
        }
        await _hotelRepository.DeleteAsync(hotel);
    }

    public async Task<PaginationResponse<HotelResponse>> SearchHotelsAsync(HotelSearchRequest request)
    {
        var (hotels, totalCount) = await _hotelRepository.SearchAsync(
            request.Query,
            request.MinPrice,
            request.MaxPrice,
            request.StarRate,
            request.RoomType,
            request.Adults,
            request.Children,
            request.PageNumber,
            request.PageSize);

        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        return new PaginationResponse<HotelResponse>
        {
            Items = hotels.Select(MapToResponse),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalPages = totalPages,
            HasPreviousPage = request.PageNumber > 1,
            HasNextPage = request.PageNumber < totalPages
        };
    }

    public async Task<HotelDetailsResponse> GetHotelDetailsAsync(int id)
    {
        var hotel = await _hotelRepository.GetByIdWithFullDetailsAsync(id);
        if (hotel == null)
        {
            throw new KeyNotFoundException($"Hotel with Id {id} not found.");
        }
        return MapToDetailsResponse(hotel);
    }

    // Helper method to map Hotel entity to HotelResponse DTO
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

    // Helper method to map Hotel entity to HotelDetailsResponse DTO, including related entities like images, reviews, and rooms
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
                Id = hi.Id,
                ImageUrl = hi.ImageUrl,
                Caption = hi.Caption
            }).ToList() ?? new List<HotelImageResponse>(),
            Reviews = hotel.Reviews?.Select(r => new ReviewResponse
            {
                Id = r.Id,
                UserId = r.UserId,
                Username = r.User?.Username ?? "Unknown User",
                HotelId = r.HotelId,
                HotelName = r.Hotel?.Name ?? "Unknown Hotel",
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedDate = r.CreatedDate
            }).ToList() ?? new List<ReviewResponse>(),
            AvailableRooms = hotel.Rooms?.Where(r => r.Availability).Select(r => new RoomResponse
            {
                Id = r.Id,
                RoomNumber = r.RoomNumber,
                RoomType = r.RoomType,
                AdultCapacity = r.AdultCapacity,
                ChildCapacity = r.ChildCapacity,
                Description = r.Description,
                PricePerNight = r.PricePerNight,
                ThumbnailUrl = r.ThumbnailUrl
            }).ToList() ?? new List<RoomResponse>()
        };
    }
}