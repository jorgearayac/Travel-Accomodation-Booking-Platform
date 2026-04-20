using HotelBooking.API.DTOs.Auth;
using HotelBooking.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    /// <returns>An AuthResponse with a JWT token, or BadRequest if username/email is taken.</returns>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var response = await _authService.RegisterUserAsync(request);

        if (response == null)
        {
            return BadRequest("Username or email already exists.");
        }
        return Ok(response);
    }

    /// <summary>
    /// Authenticates a user and returns an authentication token.
    /// </summary>
    /// <param name="request">The login credentials.</param>
    /// <returns>An AuthResponse with a JWT token, or Unauthorized if credentials are invalid.</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var response = await _authService.LoginUserAsync(request);

        if (response == null)
        {
            return Unauthorized("Invalid username or password.");
        }
        return Ok(response);
    }
}