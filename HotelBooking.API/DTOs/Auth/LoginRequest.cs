using System.ComponentModel.DataAnnotations;

namespace HotelBooking.API.DTOs.Auth;

public class LoginRequest
{
    [Required]
    public required string Username { get; set; }

    [Required]
    public required string Password { get; set; }
}
