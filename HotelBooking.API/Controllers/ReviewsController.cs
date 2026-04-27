using HotelBooking.API.DTOs.Reviews;
using HotelBooking.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("api/reviews")]
[Authorize]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewsController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpGet("hotel/{hotelId}")]
    public async Task<IActionResult> GetReviewsByHotelId(int hotelId)
    {
        var reviews = await _reviewService.GetReviewsByHotelIdAsync(hotelId);
        return Ok(reviews);
    }

    [HttpPost]
    public async Task<IActionResult> CreateReview([FromBody] CreateReviewRequest request)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var review = await _reviewService.CreateReviewAsync(userId, request);
        return CreatedAtAction(nameof(GetReviewsByHotelId), new { hotelId = review.HotelId }, review);
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteReview(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _reviewService.DeleteReviewAsync(userId, id);
        return NoContent();
    }
}
