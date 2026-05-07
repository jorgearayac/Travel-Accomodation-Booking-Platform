using HotelBooking.API.DTOs.Reviews;
using HotelBooking.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

[Route("api/reviews")]
[Authorize]
public class ReviewsController : ApiControllerBase
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
    [AllowAnonymous]
    public async Task<IActionResult> GetReviewsByHotelId(int hotelId)
    {
        var result = await _reviewService.GetReviewsByHotelIdAsync(hotelId);
        return FromResult(result);
    }

    /// <summary>
    /// Creates a review for a hotel.
    /// </summary>
    /// <param name="request">The review to create.</param>
    /// <returns>A <see cref="CreatedAtActionResult"> with the review details.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateReview([FromBody] CreateReviewRequest request)
    {
        var userId = GetCurrentUserId();
        var result = await _reviewService.CreateReviewAsync(userId, request);
        return CreatedFromResult(result, nameof(GetReviewsByHotelId), r => new { hotelId = r.HotelId });
    }

    /// <summary>
    /// Deletes a review by its Id. Only the user that made the review can delete it.
    /// </summary>
    /// <param name="id">The Id of the review to delete.</param>
    /// <returns>A <see cref="NoContentResult">.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReview(int id)
    {
        var userId = GetCurrentUserId();
        var result = await _reviewService.DeleteReviewAsync(userId, id);
        return FromResult(result);
    }
}
