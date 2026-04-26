using HotelBooking.API.DTOs.Auth;

namespace HotelBooking.API.Interfaces;

/// <summary>
/// Interface to register and authenticate users.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registers a new user and returns a token.
    /// </summary>
    /// <param name="request">The registration request.</param>
    /// <returns>AuthResponse with token, or null if registration fails.</returns>
    Task<AuthResponse> RegisterUserAsync(RegisterRequest request);
    
    /// <summary>
    /// Authenticates a user and returns a token.
    /// </summary>
    /// <param name="request">The login request.</param>
    /// <returns>AuthResponse with token, or null if authentication fails.</returns>
    Task<AuthResponse> LoginUserAsync(LoginRequest request);
}