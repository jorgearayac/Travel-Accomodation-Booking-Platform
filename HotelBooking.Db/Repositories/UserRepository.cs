using Microsoft.EntityFrameworkCore;
using HotelBooking.Db.Data;
using HotelBooking.Db.Interfaces;
using HotelBooking.Db.Models;

namespace HotelBooking.Db.Repositories;

/// <summary>
/// Repository implementation for managing user data in the database.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly HotelBookingDbContext _context;
    public UserRepository(HotelBookingDbContext context)
    {
        _context = context;
    }
    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
    public async Task<User> AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }
}
