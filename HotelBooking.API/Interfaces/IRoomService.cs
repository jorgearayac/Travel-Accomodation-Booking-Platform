using HotelBooking.API.Common;
using HotelBooking.API.DTOs.Pagination;
using HotelBooking.API.DTOs.Rooms;

namespace HotelBooking.API.Interfaces;

public interface IRoomService
{
    Task<Result<PaginationResponse<RoomResponse>>> GetAllRoomsAsync(PaginationRequest pagination);
    Task<Result<RoomResponse>> GetRoomByIdAsync(int id);
    Task<Result<RoomResponse>> CreateRoomAsync(RoomRequest request);
    Task<Result<RoomResponse>> UpdateRoomAsync(int id, RoomRequest request);
    Task<Result> DeleteRoomAsync(int id);
}
