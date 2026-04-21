using Microsoft.EntityFrameworkCore;
using HotelBooking.Db.Data;
using HotelBooking.Db.Interfaces;
using HotelBooking.Db.Models;

namespace HotelBooking.Db.Repositories;

/// <summary>
/// Repository implementation for managing user data in the database.
/// </summary>
public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(HotelBookingDbContext context) : base(context) { }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
    }
}
