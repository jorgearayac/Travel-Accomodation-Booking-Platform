using HotelBooking.API.Common;
using HotelBooking.API.DTOs.Pagination;
using HotelBooking.API.DTOs.Rooms;
using HotelBooking.API.Interfaces;
using HotelBooking.Db.Interfaces;
using HotelBooking.Db.Models;

namespace HotelBooking.API.Services;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;
    private readonly IHotelRepository _hotelRepository;
    private readonly ILogger<RoomService> _logger;

    public RoomService(
        IRoomRepository roomRepository,
        IHotelRepository hotelRepository,
        ILogger<RoomService> logger)
    {
        _roomRepository = roomRepository;
        _hotelRepository = hotelRepository;
        _logger = logger;
    }

    public async Task<Result<PaginationResponse<RoomResponse>>> GetAllRoomsAsync(PaginationRequest pagination)
    {
        var (rooms, totalCount) = await _roomRepository.GetPaginatedAsync(pagination.PageNumber, pagination.PageSize);
        var totalPages = (int)Math.Ceiling(totalCount / (double)pagination.PageSize);

        return Result<PaginationResponse<RoomResponse>>.Success(new PaginationResponse<RoomResponse>
        {
            Items = rooms.Select(MapToResponse),
            TotalCount = totalCount,
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalPages = totalPages,
            HasPreviousPage = pagination.PageNumber > 1,
            HasNextPage = pagination.PageNumber < totalPages
        });
    }

    public async Task<Result<RoomResponse>> GetRoomByIdAsync(int id)
    {
        var room = await _roomRepository.GetByIdAsync(id);
        if (room == null)
        {
            return Result<RoomResponse>.NotFound($"Room with Id {id} not found.");
        }
        return Result<RoomResponse>.Success(MapToResponse(room));
    }

    public async Task<Result<RoomResponse>> CreateRoomAsync(RoomRequest request)
    {
        var hotel = await _hotelRepository.GetByIdAsync(request.HotelId);
        if (hotel == null)
        {
            return Result<RoomResponse>.NotFound($"Hotel with Id {request.HotelId} not found.");
        }

        var room = new Room
        {
            HotelId = request.HotelId,
            RoomNumber = request.RoomNumber,
            RoomType = request.RoomType,
            AdultCapacity = request.AdultCapacity,
            ChildCapacity = request.ChildCapacity,
            Description = request.Description,
            PricePerNight = request.PricePerNight,
            Availability = request.Availability,
            ThumbnailUrl = request.ThumbnailUrl,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };
        await _roomRepository.AddAsync(room);
        _logger.LogInformation("Room {RoomNumber} created for hotel {HotelId}", request.RoomNumber, request.HotelId);
        return Result<RoomResponse>.Success(MapToResponse(room));
    }

    public async Task<Result<RoomResponse>> UpdateRoomAsync(int id, RoomRequest request)
    {
        var room = await _roomRepository.GetByIdAsync(id);
        if (room == null)
        {
            return Result<RoomResponse>.NotFound($"Room with Id {id} not found.");
        }
        room.HotelId = request.HotelId;
        room.RoomNumber = request.RoomNumber;
        room.RoomType = request.RoomType;
        room.AdultCapacity = request.AdultCapacity;
        room.ChildCapacity = request.ChildCapacity;
        room.Description = request.Description;
        room.PricePerNight = request.PricePerNight;
        room.Availability = request.Availability;
        room.ThumbnailUrl = request.ThumbnailUrl;
        room.UpdatedDate = DateTime.UtcNow;

        await _roomRepository.UpdateAsync(room);
        return Result<RoomResponse>.Success(MapToResponse(room));
    }

    public async Task<Result> DeleteRoomAsync(int id)
    {
        var room = await _roomRepository.GetByIdAsync(id);
        if (room == null)
        {
            return Result.NotFound($"Room with Id {id} not found.");
        }
        await _roomRepository.DeleteAsync(room);
        _logger.LogInformation("Room {RoomId} deleted", id);
        return Result.Success();
    }

    private RoomResponse MapToResponse(Room room)
    {
        return new RoomResponse
        {
            Id = room.Id,
            HotelId = room.HotelId,
            RoomNumber = room.RoomNumber,
            RoomType = room.RoomType,
            AdultCapacity = room.AdultCapacity,
            ChildCapacity = room.ChildCapacity,
            Description = room.Description,
            PricePerNight = room.PricePerNight,
            Availability = room.Availability,
            ThumbnailUrl = room.ThumbnailUrl,
            CreatedDate = room.CreatedDate,
            UpdatedDate = room.UpdatedDate
        };
    }
}
