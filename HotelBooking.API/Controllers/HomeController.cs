using HotelBooking.API.Interfaces;
using HotelBooking.API.Services;
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

    [HttpGet("featured-deals")]
    public async Task<IActionResult> GetFeaturedDeals()
    {
        var deals = await _featuredDealService.GetAllFeaturedDealsAsync();
        return Ok(deals);
    }

    [HttpGet("recently-booked")]
    public async Task<IActionResult> GetRecentlyBooked()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var recentHotels = await _bookingService.GetRecentlyBookedHotelsAsync(userId);
        return Ok(recentHotels);
    }

    [HttpGet("trending-destinations")]
    public async Task<IActionResult> GetTrendingDestinations()
    {
        var destinations = await _cityService.GetTrendingDestinationsAsync();
        return Ok(destinations);
    }
}