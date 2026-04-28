using HotelBooking.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("api/home")]
[Authorize]
public class HomeController : ControllerBase
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
    public async Task<IActionResult> GetFeaturedDeals()
    {
        var deals = await _featuredDealService.GetAllFeaturedDealsAsync();
        return Ok(deals);
    }

    /// <summary>
    /// Retrieves a list of hotels that the current user has recently booked.
    /// </summary>
    /// <returns>An <see cref="OkObjectResult"/> containing a collection of recently booked hotels. Otherwise, an empty list.</returns>
    [HttpGet("recently-booked")]
    public async Task<IActionResult> GetRecentlyBooked()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var recentHotels = await _bookingService.GetRecentlyBookedHotelsAsync(userId);
        return Ok(recentHotels);
    }

    /// <summary>
    /// Retrieves a list of trending travel destinations.
    /// </summary>\
    /// <returns>An <see cref="OkObjectResult"/> containing a collection of trending destinations. Otherwise, an empty list.</returns>
    [HttpGet("trending-destinations")]
    public async Task<IActionResult> GetTrendingDestinations()
    {
        var destinations = await _cityService.GetTrendingDestinationsAsync();
        return Ok(destinations);
    }
}