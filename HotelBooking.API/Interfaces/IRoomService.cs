using HotelBooking.API.DTOs.Pagination;
using HotelBooking.API.DTOs.Rooms;

namespace HotelBooking.API.Interfaces;

public interface IRoomService
{
    Task<PaginationResponse<RoomResponse>> GetAllRoomsAsync(PaginationRequest pagination);
    Task<RoomResponse> GetRoomByIdAsync(int id);
    Task<RoomResponse> CreateRoomAsync(RoomRequest request);
    Task<RoomResponse> UpdateRoomAsync(int id, RoomRequest request);
    Task DeleteRoomAsync(int id);
}
