using HotelBooking.Db.Models;

namespace HotelBooking.Db.Interfaces;

/// <summary>
/// Repository interface for managing city data in the database.
/// </summary>
public interface ICityRepository : IRepository<City>
{
    // Empty for now
    // All CRUD comes from IRepository<City>
}