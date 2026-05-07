using HotelBooking.API.DTOs.Cities;
using HotelBooking.API.DTOs.Pagination;
using HotelBooking.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

[Route("api/cities")]
[Authorize]
public class CitiesController : ApiControllerBase
{
    private readonly ICityService _cityService;

    public CitiesController(ICityService cityService)
    {
        _cityService = cityService;
    }

    /// <summary>
    /// Returns a list of all cities available in the system.
    /// </summary>
    /// <returns>An <see cref="OkObjectResult"/> with a list of cities.</returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllCities([FromQuery] PaginationRequest pagination)
    {
        var result = await _cityService.GetAllCitiesAsync(pagination);
        return FromResult(result);
    }

    /// <summary>
    /// Retrieves the details of a city by its Id.
    /// </summary>
    /// <param name="id">The Id of the city to search for.</param>
    /// <returns>An <see cref="OkObjectResult"/> with the details of the city, or a <see cref="NotFoundResult"/> if the city does not exist.</returns>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCityById(int id)
    {
        var result = await _cityService.GetCityByIdAsync(id);
        return FromResult(result);
    }

    /// <summary>
    /// Creates a new city in the system. Only users with the "Admin" role can perform this action.
    /// </summary>
    /// <param name="request">The details of the city to create.</param>
    /// <returns>A <see cref="CreatedAtActionResult"/> with the created city.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateCity([FromBody] CityRequest request)
    {
        var result = await _cityService.CreateCityAsync(request);
        return CreatedFromResult(result, nameof(GetCityById), c => new { id = c.Id });
    }

    /// <summary>
    /// Updates the details of an existing city with its Id. Only users with the "Admin" role can perform this action.
    /// </summary>
    /// <param name="id">The Id of the city to update.</param>
    /// <param name="request">The updated details of the city.</param>
    /// <returns>An <see cref="OkObjectResult"/> with the updated city, or a <see cref="NotFoundResult"/> if the city does not exist.</returns>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateCity(int id, [FromBody] CityRequest request)
    {
        var result = await _cityService.UpdateCityAsync(id, request);
        return FromResult(result);
    }

    /// <summary>
    /// Deletes a city from the system by its Id. Only users with the "Admin" role can perform this action.
    /// </summary>
    /// <param name="id">The Id of the city to delete.</param>
    /// <returns>A <see cref="NoContentResult"/> if the deletion is successful, or a <see cref="NotFoundResult"/> if the city does not exist.</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCity(int id)
    {
        var result = await _cityService.DeleteCityAsync(id);
        return FromResult(result);
    }
}
