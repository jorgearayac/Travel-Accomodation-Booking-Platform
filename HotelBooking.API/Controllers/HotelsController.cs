using HotelBooking.API.DTOs.Hotels;
using HotelBooking.API.DTOs.Pagination;
using HotelBooking.API.DTOs.Search;
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
    private readonly IHotelImageService _hotelImageService;
    public HotelsController(IHotelService hotelService, IHotelImageService hotelImageService)
    {
        _hotelService = hotelService;
        _hotelImageService = hotelImageService;
    }

    /// <summary>
    /// Returns a list of all hotels available in the system.
    /// </summary>
    /// <returns>An <see cref="OkObjectResult"/> with a list of hotels.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllHotels([FromQuery] PaginationRequest pagination)
    {
        var hotels = await _hotelService.GetAllHotelsAsync(pagination);
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

    // Search endpoint for hotels based on various criteria
    [HttpGet("search")]
    public async Task<IActionResult> SearchHotels([FromQuery] HotelSearchRequest request)
    {
        var results = await _hotelService.SearchHotelsAsync(request);
        return Ok(results);
    }

    // Endpoint to get hotel details including images and rooms
    [HttpGet("{id}/details")]
    public async Task<IActionResult> GetHotelDetails(int id)
    {
        try
        {
            var hotel = await _hotelService.GetHotelDetailsAsync(id);
            return Ok(hotel);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // Endpoint to get all images for a specific hotel
    [HttpGet("{id}/images")]
    public async Task<IActionResult> GetHotelImages(int id)
    {
        var images = await _hotelImageService.GetImagesByHotelIdAsync(id);
        return Ok(images);
    }

    // Endpoint to create a new image for a specific hotel. Only users with the "Admin" role can perform this action.
    [HttpPost("{id}/images")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateHotelImage(int id, [FromBody] HotelImageRequest request)
    {
        request.HotelId = id; // Ensure the hotel Id is set from the route parameter
        var image = await _hotelImageService.CreateImageAsync(request);
        return CreatedAtAction(nameof(GetHotelImages), new { id }, image);
    }

    // Endpoint to update an existing hotel image. Only users with the "Admin" role can perform this action.
    [HttpPut("images/{imageId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateHotelImage(int imageId, [FromBody] HotelImageRequest request)
    {
        try
        {
            var image = await _hotelImageService.UpdateImageAsync(imageId, request);
            return Ok(image);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // Endpoint to delete a hotel image. Only users with the "Admin" role can perform this action.
    [HttpDelete("images/{imageId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteHotelImage(int imageId)
    {
        try
        {
            await _hotelImageService.DeleteImageAsync(imageId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}