using HotelBooking.Db.Models;

namespace HotelBooking.Db.Interfaces;

/// <summary>
/// Repository interface for managing user data in the database.
/// </summary>
public interface IUserRepository : IRepository<User>
{
    /// <summary>
    /// Finds a user by their username.
    /// </summary>
    /// <param name="username">The username to search for.</param>
    /// <returns>The user, or null if not found.</returns>
    Task<User?> GetByUsernameAsync(string username);

    /// <summary>
    /// Finds a user by their email address.
    /// </summary>
    /// <param name="email">The email to search for.</param>
    /// <returns>The user, or null if not found.</returns>
    Task<User?> GetByEmailAsync(string email);
}
