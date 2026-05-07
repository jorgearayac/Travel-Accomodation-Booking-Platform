using HotelBooking.API.DTOs.Pagination;
using HotelBooking.API.DTOs.Rooms;
using HotelBooking.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

[Route("api/rooms")]
[Authorize]
public class RoomsController : ApiControllerBase
{
    private readonly IRoomService _roomService;
    public RoomsController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    /// <summary>
    /// Retrieves all rooms paginated.
    /// </summary>
    /// <param name="pagination">The paginated request with the details of the page.</param>
    /// <returns>An <see cref="OkObjectResult"> with a list of the rooms and its details.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllRooms([FromQuery] PaginationRequest pagination)
    {
        var result = await _roomService.GetAllRoomsAsync(pagination);
        return FromResult(result);
    }

    /// <summary>
    /// Retrieves a room by its Id.
    /// </summary>
    /// <param name="id">The Id of the room to search for.</param>
    /// <returns>An <see cref="OkObjectResult"> with the room details.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoomById(int id)
    {
        var result = await _roomService.GetRoomByIdAsync(id);
        return FromResult(result);
    }

    /// <summary>
    /// Creates a room. Only users with role "Admin" can perform this action.
    /// </summary>
    /// <param name="request">The details of the room to create.</param>
    /// <returns>An <see cref="CreatedAtActionResult"> with the room details.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateRoom([FromBody] RoomRequest request)
    {
        var result = await _roomService.CreateRoomAsync(request);
        return CreatedFromResult(result, nameof(GetRoomById), r => new { id = r.Id });
    }

    /// <summary>
    /// Updates a room by its Id. Only users with role "Admin" can perform this action.
    /// </summary>
    /// <param name="id">The Id of the room to update.</param>
    /// <param name="request">The details of the updated room.</param>
    /// <returns>An <see cref="OkObjectResult"> with the room details.</returns>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateRoom(int id, [FromBody] RoomRequest request)
    {
        var result = await _roomService.UpdateRoomAsync(id, request);
        return FromResult(result);
    }

    /// <summary>
    /// Deletes a room by its Id. Only users with role "Admin" can perform this action.
    /// </summary>
    /// <param name="id">The Id of the room to delete.</param>
    /// <returns>A <see cref="NoContentResult">.</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteRoom(int id)
    {
        var result = await _roomService.DeleteRoomAsync(id);
        return FromResult(result);
    }
}