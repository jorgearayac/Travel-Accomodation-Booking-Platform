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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookingById(int id)
    {
        try
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            return Ok(booking);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetBookingByUser()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var bookings = await _bookingService.GetBookingsByUserAsync(userId);
        return Ok(bookings);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var booking = await _bookingService.CreateBookingAsync(userId, request);
            return CreatedAtAction(nameof(GetBookingById), new { id = booking.Id }, booking);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }
}