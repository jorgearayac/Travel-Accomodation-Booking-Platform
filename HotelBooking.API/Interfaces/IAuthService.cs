using HotelBooking.API.Common;
using HotelBooking.API.DTOs.Auth;

namespace HotelBooking.API.Interfaces;

public interface IAuthService
{
    Task<Result<AuthResponse>> RegisterUserAsync(RegisterRequest request);
    Task<Result<AuthResponse>> LoginUserAsync(LoginRequest request);
}
