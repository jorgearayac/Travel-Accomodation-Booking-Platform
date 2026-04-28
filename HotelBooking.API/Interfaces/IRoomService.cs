using HotelBooking.API.DTOs.Pagination;
using HotelBooking.API.DTOs.Rooms;

namespace HotelBooking.API.Interfaces;

/// <summary>
/// Interface for room-related operations in the system.
/// </summary>
public interface IRoomService
{
    /// <summary>
    /// Retrieves all rooms with pagination.
    /// </summary>
    /// <param name="pagination">The page details.</param>
    /// <returns> with the details of the page.</returns>
    Task<PaginationResponse<RoomResponse>> GetAllRoomsAsync(PaginationRequest pagination);

    /// <summary>
    /// Retrieves a room by its Id.
    /// </summary>
    /// <param name="id">The Id of the room to search for.</param>
    /// <returns>RoomResponse, null if not found.</returns>
    Task<RoomResponse> GetRoomByIdAsync(int id);

    /// <summary>
    /// Creates a room.
    /// </summary>
    /// <param name="request">The details of the room to create.</param>
    /// <returns>RoomResponse.</returns>
    Task<RoomResponse> CreateRoomAsync(RoomRequest request);

    /// <summary>
    /// Updates a room by its Id.
    /// </summary>
    /// <param name="id">The Id of the room to update.</param>
    /// <param name="request">The details of the updated room.</param>
    /// <returns>RoomResponse.</returns>
    Task<RoomResponse> UpdateRoomAsync(int id, RoomRequest request);

    /// <summary>
    /// Deletes a room by its Id.
    /// </summary>
    /// <param name="id">The Id of the room to delete.</param>
    /// <returns></returns>
    Task DeleteRoomAsync(int id);
}
