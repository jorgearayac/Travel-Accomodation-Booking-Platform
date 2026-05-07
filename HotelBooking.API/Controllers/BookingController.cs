using HotelBooking.API.DTOs.Bookings;
using HotelBooking.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

[Route("api/booking")]
[Authorize]
public class BookingController : ApiControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    /// <summary>
    /// Searchs and returns a booking by its Id. Only the owner can view their booking.
    /// </summary>
    /// <param name="id">The Id to search for.</param>
    /// <returns>An <see cref="OkObjectResult"/> with the searched booking.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookingById(int id)
    {
        var userId = GetCurrentUserId();
        var result = await _bookingService.GetBookingByIdAsync(id, userId);
        return FromResult(result);
    }

    /// <summary>
    /// Searchs and returns a booking by its current user.
    /// </summary>
    /// <returns>An <see cref="OkObjectResult"/> with the searched booking.</returns>
    [HttpGet("user")]
    public async Task<IActionResult> GetBookingByUser()
    {
        var userId = GetCurrentUserId();
        var result = await _bookingService.GetBookingsByUserAsync(userId);
        return FromResult(result);
    }

    /// <summary>
    /// Creates a new booking using the specified booking request data.
    /// </summary>
    /// <param name="request">The booking details to create. Must not be null.</param>
    /// <returns>A <see cref="CreatedAtActionResult"> response containing the newly created booking resource.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
    {
        var userId = GetCurrentUserId();
        var result = await _bookingService.CreateBookingAsync(userId, request);
        return CreatedFromResult(result, nameof(GetBookingById), b => new { id = b.Id });
    }
}
