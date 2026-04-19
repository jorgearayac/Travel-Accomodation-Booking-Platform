namespace HotelBooking.API.DTOs.Auth;

public class AuthResponse
{
    public required string Token { get; set; }
    public int UserId { get; set; }
    public required string Username { get; set; }
    public required string Role { get; set; }
}