using HotelBooking.API.DTOs.Hotels;
using HotelBooking.API.DTOs.Rooms;
using HotelBooking.API.Interfaces;
using HotelBooking.Db.Enums;
using HotelBooking.Db.Interfaces;
using HotelBooking.Db.Models;
using HotelBooking.Db.Repositories;

namespace HotelBooking.API.Services;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;

    public RoomService(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<IEnumerable<RoomResponse>> GetAllRoomsAsync()
    {
        var rooms = await _roomRepository.GetAllAsync();
        return rooms.Select(MapToResponse);
    }

    public async Task<RoomResponse> GetRoomByIdAsync(int id)
    {
        var room = await _roomRepository.GetByIdAsync(id);
        if (room == null)
        {
            throw new KeyNotFoundException($"Room with Id {id} not found.");
        }
        return MapToResponse(room);
    }

    public async Task<RoomResponse> CreateRoomAsync(RoomRequest request)
    {
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
        return MapToResponse(room);
    }
    
    public async Task<RoomResponse> UpdateRoomAsync(int id, RoomRequest request)
    {
        var room = await _roomRepository.GetByIdAsync(id);
        if (room == null)
        {
            throw new KeyNotFoundException($"Room with Id {id} not found.");
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
        return MapToResponse(room);
    }

    public async Task DeleteRoomAsync(int id)
    {
        var room = await _roomRepository.GetByIdAsync(id);
        if (room == null)
        {
            throw new KeyNotFoundException($"Room with Id {id} not found.");
        }
        await _roomRepository.DeleteAsync(room);
    }

    // Helper method to map Room entity to RoomResponse DTO
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