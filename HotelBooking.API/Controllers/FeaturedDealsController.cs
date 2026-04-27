using HotelBooking.API.DTOs.FeaturedDeals;
using HotelBooking.API.Interfaces;
using HotelBooking.API.Services;
using HotelBooking.Db.Models;
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

    [HttpGet]
    public async Task<IActionResult> GetAllFeaturedDeals()
    {
        var deals = await _featuredDealService.GetAllFeaturedDealsAsync();
        return Ok(deals);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFeaturedDealById(int id)
    {
        var deal = await _featuredDealService.GetFeaturedDealByIdAsync(id);
        return Ok(deal);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateFeaturedDeal([FromBody] FeaturedDealRequest request)
    {
        var deal = await _featuredDealService.CreateFeaturedDealAsync(request);
        return CreatedAtAction(nameof(GetFeaturedDealById), new { id = deal.Id }, deal);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateFeaturedDeal(int id, [FromBody] FeaturedDealRequest request)
    {
        var deal = await _featuredDealService.UpdateFeaturedDealAsync(id, request);
        return Ok(deal);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteFeaturedDeal(int id)
    {
        await _featuredDealService.DeleteFeaturedDealAsync(id);
        return NoContent();
    }
}