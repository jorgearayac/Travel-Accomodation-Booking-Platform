using HotelBooking.API.DTOs.Bookings;
using HotelBooking.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("api/booking")]
[Authorize]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    /// <summary>
    /// Searchs and returns a booking by its Id.
    /// </summary>
    /// <param name="id">The Id to search for.</param>
    /// <returns>An <see cref="OkObjectResult"/> with the searched booking.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookingById(int id)
    {
        var booking = await _bookingService.GetBookingByIdAsync(id);
        return Ok(booking);
    }

    /// <summary>
    /// Searchs and returns a booking by its current user.
    /// </summary>
    /// <returns>An <see cref="OkObjectResult"/> with the searched booking.</returns>
    [HttpGet("user")]
    public async Task<IActionResult> GetBookingByUser()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var bookings = await _bookingService.GetBookingsByUserAsync(userId);
        return Ok(bookings);
    }

    /// <summary>
    /// Creates a new booking using the specified booking request data.
    /// </summary>
    /// <param name="request">The booking details to create. Must not be null.</param>
    /// <returns>A <see cref="CreatedAtActionResult"> response containing the newly created booking resource.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var booking = await _bookingService.CreateBookingAsync(userId, request);
        return CreatedAtAction(nameof(GetBookingById), new { id = booking.Id }, booking);
    }
}