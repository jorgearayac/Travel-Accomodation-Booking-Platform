using HotelBooking.API.DTOs.Auth;
using HotelBooking.API.Interfaces;
using HotelBooking.Db.Enums;
using HotelBooking.Db.Interfaces;
using HotelBooking.Db.Models;

namespace HotelBooking.API.Services;

/// <summary>
/// Authentication service that handles user registration and login.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public AuthService(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse?> RegisterUserAsync(RegisterRequest request)
    {
        var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
        if (existingUser != null)
        {
            return null;
        }

        var existingEmail = await _userRepository.GetByEmailAsync(request.Email);
        if (existingEmail != null)
        {
            return null;
        }

        var user = new User // refactor later
        {
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Role = UserRole.User,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);
        return BuildAuthResponse(user);
    }

    public async Task<AuthResponse?> LoginUserAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return null;
        }
        return BuildAuthResponse(user);
    }
    
    // Helper method, refactor later
    private AuthResponse BuildAuthResponse(User user)
    {
        var token = _tokenService.GenerateToken(user);
        return new AuthResponse
        {
            Token = token,
            UserId = user.Id,
            Username = user.Username,
            Role = user.Role.ToString()
        };
    }
}