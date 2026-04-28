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

    /// <summary>
    /// Retrieves all reviews of a hotel by its Id.
    /// </summary>
    /// <param name="hotelId">The Id of the hotel to look for.</param>
    /// <returns>An <see cref="OkObjectResult"> with the reviews of the hotel.</returns>
    [HttpGet("hotel/{hotelId}")]
    public async Task<IActionResult> GetReviewsByHotelId(int hotelId)
    {
        var reviews = await _reviewService.GetReviewsByHotelIdAsync(hotelId);
        return Ok(reviews);
    }

    /// <summary>
    /// Creates a review for a hotel.
    /// </summary>
    /// <param name="request">The review to create.</param>
    /// <returns>A <see cref="CreatedAtActionResult"> with the review details.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateReview([FromBody] CreateReviewRequest request)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var review = await _reviewService.CreateReviewAsync(userId, request);
        return CreatedAtAction(nameof(GetReviewsByHotelId), new { hotelId = review.HotelId }, review);
    }

    /// <summary>
    /// Deletes a reviw by its Id. Only the user that made the review can delete it.
    /// </summary>
    /// <param name="id">The Id of the review to delete.</param>
    /// <returns>A <see cref="NoContentResult">.</returns>
    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteReview(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _reviewService.DeleteReviewAsync(userId, id);
        return NoContent();
    }
}
