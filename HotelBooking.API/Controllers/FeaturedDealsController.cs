using HotelBooking.API.DTOs.FeaturedDeals;
using HotelBooking.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("api/featured-deals")]
[Authorize]
public class FeaturedDealsController : ControllerBase
{
    private readonly IFeaturedDealService _featuredDealService;
    public FeaturedDealsController(IFeaturedDealService featuredDealService)
    {
        _featuredDealService = featuredDealService;
    }

    /// <summary>
    /// Retrieves all featured deals.
    /// </summary>
    /// <returns>An <see cref="OkObjectResult"> with the deals.</see></returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllFeaturedDeals()
    {
        var deals = await _featuredDealService.GetAllFeaturedDealsAsync();
        return Ok(deals);
    }

    /// <summary>
    /// Retrieves the featured deal that matches the specified identifier.
    /// </summary>
    /// <param name="id">The Id of the featured deal to search for.</param>
    /// <returns>An <see cref="OkObjectResult"> containing the featured deal if found; 
    /// otherwise, a <see cref="NotFoundObjectResult"> result.</returns>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetFeaturedDealById(int id)
    {
        var deal = await _featuredDealService.GetFeaturedDealByIdAsync(id);
        return Ok(deal);
    }

    /// <summary>
    /// Creates a new featured deal using the specified request data. Only users with role "Admin" can do this action.
    /// </summary>
    /// <param name="request">The details of the featured deal to create. Must not be null.</param>
    /// <returns>A <see cref="CreatedAtActionResult"> response containing the created featured deal.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateFeaturedDeal([FromBody] FeaturedDealRequest request)
    {
        var deal = await _featuredDealService.CreateFeaturedDealAsync(request);
        return CreatedAtAction(nameof(GetFeaturedDealById), new { id = deal.Id }, deal);
    }

    /// <summary>
    /// Updates the details of an existing deal by its Id. Only users with role "Admin" can do this action.
    /// </summary>
    /// <param name="id">The Id of the featured deal to update.</param>
    /// <param name="request">The details of the featured deal to update. Must not be null.</param>
    /// <returns>An <see cref="OkObjectResult"> containing the updated featured deal if the operation is successful; 
    /// otherwise, a <see cref="NotFoundObjectResult">.</returns>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateFeaturedDeal(int id, [FromBody] FeaturedDealRequest request)
    {
        var deal = await _featuredDealService.UpdateFeaturedDealAsync(id, request);
        return Ok(deal);
    }

    /// <summary>
    /// Deletes the featured deal with the specified identifier. Only users with role "Admin" can do this action.
    /// </summary>
    /// <param name="id">The Id of the featured deal to delete.</param>
    /// <returns>A <see cref="NoContentResult"> indicating that the operation completed successfully with no content.</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteFeaturedDeal(int id)
    {
        await _featuredDealService.DeleteFeaturedDealAsync(id);
        return NoContent();
    }
}