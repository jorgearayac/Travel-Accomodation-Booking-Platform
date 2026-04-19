using HotelBooking.Db.Models;

namespace HotelBooking.Db.Interfaces;

/// <summary>
/// Repository interface for managing user data in the database.
/// </summary>
public interface IUserRepository
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

    /// <summary>
    /// Adds a new user to the database.
    /// </summary>
    /// <param name="user">The user to create.</param>
    /// <returns>The created user with the generated Id.</returns>
    Task<User> AddAsync(User user);
}
