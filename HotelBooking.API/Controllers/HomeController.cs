using HotelBooking.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

[Route("api/home")]
public class HomeController : ApiControllerBase
{
    private readonly IFeaturedDealService _featuredDealService;
    private readonly IBookingService _bookingService;
    private readonly ICityService _cityService;

    public HomeController(IFeaturedDealService featuredDealService, IBookingService bookingService, ICityService cityService)
    {
        _featuredDealService = featuredDealService;
        _bookingService = bookingService;
        _cityService = cityService;
    }

    /// <summary>
    /// Retrieves a list of featured deals.
    /// </summary>
    /// <returns>An <see cref="OkObjectResult"/> containing a collection of featured deals for booking.</returns>
    [HttpGet("featured-deals")]
    [AllowAnonymous]
    public async Task<IActionResult> GetFeaturedDeals()
    {
        var result = await _featuredDealService.GetAllFeaturedDealsAsync();
        return FromResult(result);
    }

    /// <summary>
    /// Retrieves a list of hotels that the current user has recently booked.
    /// </summary>
    /// <returns>An <see cref="OkObjectResult"/> containing a collection of recently booked hotels. Otherwise, an empty list.</returns>
    [HttpGet("recently-booked")]
    [Authorize]
    public async Task<IActionResult> GetRecentlyBooked()
    {
        var userId = GetCurrentUserId();
        var result = await _bookingService.GetRecentlyBookedHotelsAsync(userId);
        return FromResult(result);
    }

    /// <summary>
    /// Retrieves a list of trending travel destinations.
    /// </summary>
    /// <returns>An <see cref="OkObjectResult"/> containing a collection of trending destinations. Otherwise, an empty list.</returns>
    [HttpGet("trending-destinations")]
    [AllowAnonymous]
    public async Task<IActionResult> GetTrendingDestinations()
    {
        var result = await _cityService.GetTrendingDestinationsAsync();
        return FromResult(result);
    }
}
