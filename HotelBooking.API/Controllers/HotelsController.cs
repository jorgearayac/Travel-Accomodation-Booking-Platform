using HotelBooking.API.DTOs.Hotels;
using HotelBooking.API.DTOs.Pagination;
using HotelBooking.API.DTOs.Search;
using HotelBooking.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

[Route("api/hotels")]
[Authorize]
public class HotelsController : ApiControllerBase
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
    [AllowAnonymous]
    public async Task<IActionResult> GetAllHotels([FromQuery] PaginationRequest pagination)
    {
        var result = await _hotelService.GetAllHotelsAsync(pagination);
        return FromResult(result);
    }

    /// <summary>
    /// Retrieves the details of a specific hotel by its Id.
    /// </summary>
    /// <param name="id">The Id of the hotel to retrieve.</param>
    /// <returns>An <see cref="OkObjectResult"/> with the hotel details, or <see cref="NotFoundResult"/> if the hotel does not exist.</returns>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetHotelById(int id)
    {
        var result = await _hotelService.GetHotelByIdAsync(id);
        return FromResult(result);
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
        var result = await _hotelService.CreateHotelAsync(request);
        return CreatedFromResult(result, nameof(GetHotelById), h => new { id = h.Id });
    }

    /// <summary>
    /// Updates a hotel in the system. Only users with the "Admin" role can perform this action.
    /// </summary>
    /// <param name="id">The Id of the hotel to update.</param>
    /// <param name="request">The updated hotel details.</param>
    /// <returns>An <see cref="OkObjectResult"/> with the updated hotel, or <see cref="NotFoundResult"/> if the hotel does not exist.</returns>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateHotel(int id, [FromBody] HotelRequest request)
    {
        var result = await _hotelService.UpdateHotelAsync(id, request);
        return FromResult(result);
    }

    /// <summary>
    /// Deletes a hotel from the system. Only users with the "Admin" role can perform this action.
    /// </summary>
    /// <param name="id">The Id of the hotel to delete.</param>
    /// <returns>A <see cref="NoContentResult"/> if the deletion is successful, or <see cref="NotFoundResult"/> if the hotel does not exist.</returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteHotel(int id)
    {
        var result = await _hotelService.DeleteHotelAsync(id);
        return FromResult(result);
    }

    /// <summary>
    /// Searchs a hotel with various filter criteria.
    /// </summary>
    /// <param name="request">The search request with its filters.</param>
    /// <returns>An <see cref="OkObjectResult"/> with the filtered search. Or an empty list if the filters does not match any hotel.</returns>
    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<IActionResult> SearchHotels([FromQuery] HotelSearchRequest request)
    {
        var result = await _hotelService.SearchHotelsAsync(request);
        return FromResult(result);
    }

    /// <summary>
    /// Retrieves detailed information for the specified hotel.
    /// </summary>
    /// <param name="id">The Id of the hotel to retrieve details for.</param>
    /// <returns>An <see cref="OkObjectResult"/> containing the hotel details if found; otherwise, a not found result.</returns>
    [HttpGet("{id:int}/details")]
    [AllowAnonymous]
    public async Task<IActionResult> GetHotelDetails(int id)
    {
        var result = await _hotelService.GetHotelDetailsAsync(id);
        return FromResult(result);
    }

    /// <summary>
    /// Retrieves all images of a specific hotel by its Id.
    /// </summary>
    /// <param name="id">The Id of the hotel to retrieves images for.</param>
    /// <returns>An <see cref="OkObjectResult"/> containing the hotel images if found; otherwise, an empty list.</returns>
    [HttpGet("{id:int}/images")]
    [AllowAnonymous]
    public async Task<IActionResult> GetHotelImages(int id)
    {
        var result = await _hotelImageService.GetImagesByHotelIdAsync(id);
        return FromResult(result);
    }

    /// <summary>
    /// Creates an image for a specific hotel by its Id from the route. Only users with role "Admin" can do this action.
    /// </summary>
    /// <param name="id">The Id of the hotel to create an image. </param>
    /// <param name="request">The image request with its details.</param>
    /// <returns>A <see cref="CreatedAtActionResult"/> with the details of the image.</returns>
    [HttpPost("{id:int}/images")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateHotelImage(int id, [FromBody] HotelImageRequest request)
    {
        request.HotelId = id;
        var result = await _hotelImageService.CreateImageAsync(request);
        return CreatedFromResult(result, nameof(GetHotelImages), _ => new { id });
    }

    /// <summary>
    /// Updates an image by its Id. Only users with role "Admin" can do this action.
    /// </summary>
    /// <param name="imageId">The Id of the image to update.</param>
    /// <param name="request">The updated image with its details.</param>
    /// <returns>An <see cref="OkObjectResult"/> with the details of the updated image.</returns>
    [HttpPut("images/{imageId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateHotelImage(int imageId, [FromBody] HotelImageRequest request)
    {
        var result = await _hotelImageService.UpdateImageAsync(imageId, request);
        return FromResult(result);
    }

    /// <summary>
    /// Deletes an image by its Id. Only users with role "Admin" can do this action.
    /// </summary>
    /// <param name="imageId">The Id of the image to delete.</param>
    /// <returns>A <see cref="NoContentResult"/>.</returns>
    [HttpDelete("images/{imageId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteHotelImage(int imageId)
    {
        var result = await _hotelImageService.DeleteImageAsync(imageId);
        return FromResult(result);
    }
}
