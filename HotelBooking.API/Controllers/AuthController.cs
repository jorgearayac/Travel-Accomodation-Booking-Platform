using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelBooking.API.DTOs.Auth;
using HotelBooking.API.Interfaces;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Registers a new user and returns an authentication token.
    /// </summary>
    /// <param name="request">The registration details.</param>
    /// <returns>An <see cref="OkObjectResult"/> with an AuthResponse containing a JWT token, or a <see cref="BadRequestObjectResult"/> if username/email is taken.</returns>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        try
        {
            var response = await _authService.RegisterUserAsync(request);
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Authenticates a user and returns an authentication token.
    /// </summary>
    /// <param name="request">The login credentials.</param>
    /// <returns>An <see cref="OkObjectResult"/> with an AuthResponse containing a JWT token, or a <see cref="UnauthorizedResult"/> if credentials are invalid.</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        try
        {
            var response = await _authService.LoginUserAsync(request);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }
}