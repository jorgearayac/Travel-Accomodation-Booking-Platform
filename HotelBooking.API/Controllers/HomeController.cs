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

    public HomeController(IFeaturedDealService featuredDealService, IBookingService bookingService)
    {
        _featuredDealService = featuredDealService;
        _bookingService = bookingService;
    }

    // GET /api/home/featured-deals
    [HttpGet("featured-deals")]
    public async Task<IActionResult> GetFeaturedDeals()
    {
        var deals = await _featuredDealService.GetAllFeaturedDealsAsync();
        return Ok(deals);
    }

    // GET /api/home/recently-booked
    [HttpGet("recently-booked")]
    public async Task<IActionResult> GetRecentlyBooked()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var recentHotels = await _bookingService.GetRecentlyBookedHotelsAsync(userId); 
        return Ok(recentHotels);
    }

    // GET /api/home/trending-destinations
}