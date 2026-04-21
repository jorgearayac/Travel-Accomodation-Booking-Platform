using HotelBooking.API.DTOs.Hotels;
using HotelBooking.API.Interfaces;
using HotelBooking.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("api/hotels")]
[Authorize]
public class HotelsController : ControllerBase
{
    private readonly IHotelService _hotelService;
    public HotelsController(IHotelService hotelService)
    {
        _hotelService = hotelService;
    }

    /// <summary>
    /// Returns a list of all hotels available in the system.
    /// </summary>
    /// <returns>An <see cref="OkObjectResult"/> with a list of hotels.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllHotels()
    {
        var hotels = await _hotelService.GetAllHotelsAsync();
        return Ok(hotels);
    }

    /// <summary>
    /// Retrieves the details of a specific hotel by its Id.
    /// </summary>
    /// <param name="id">The Id of the hotel to retrieve.</param>
    /// <returns>An <see cref="OkObjectResult"/> with the hotel details, or <see cref="NotFoundResult"/> if the hotel does not exist.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetHotelById(int id)
    {
        try
        {
            var hotel = await _hotelService.GetHotelByIdAsync(id);
            return Ok(hotel);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Creates a new hotel in the system. Only users with the "Admin" role can perform this action.
    /// </summary>
    /// <param name="request">The hotel details to create.</param>
    /// <returns>A <see cref="CreatedAtActionResult"/> with the created hotel.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateHotel([FromBody] HotelRequest request)
    {
        var hotel = await _hotelService.CreateHotelAsync(request);
        return CreatedAtAction(nameof(GetHotelById), new { id = hotel.Id }, hotel);
    }

    /// <summary>
    /// Updates a hotel in the system. Only users with the "Admin" role can perform this action.
    /// </summary>
    /// <param name="id">The Id of the hotel to update.</param>
    /// <param name="request">The updated hotel details.</param>
    /// <returns>An <see cref="OkObjectResult"/> with the updated hotel, or <see cref="NotFoundResult"/> if the hotel does not exist.</returns>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateHotel(int id, [FromBody] HotelRequest request)
    {
        try
        {
            var hotel = await _hotelService.UpdateHotelAsync(id, request);
            return Ok(hotel);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Deletes a hotel from the system. Only users with the "Admin" role can perform this action.
    /// </summary>
    /// <param name="id">The Id of the hotel to delete.</param>
    /// <returns>A <see cref="NoContentResult"/> if the deletion is successful, or <see cref="NotFoundResult"/> if the hotel does not exist.</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteHotel(int id)
    {
        try
        {
            await _hotelService.DeleteHotelAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}