using HotelBooking.Db.Models;

namespace HotelBooking.API.Interfaces;

/// <summary>
/// Interface for generating authentication tokens for users.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Generates a security token for the specified user.
    /// </summary>
    /// <param name="user">The user for whom to generate the token. Cannot be null.</param>
    /// <returns>A string containing the generated security token for the user.</returns>
    string GenerateToken(User user);
}
