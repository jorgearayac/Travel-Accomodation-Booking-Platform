using HotelBooking.Db.Models;

namespace HotelBooking.Db.Interfaces;

/// <summary>
/// Featured deal repository for managing the data in the database.
/// </summary>
public interface IFeaturedDealRepository : IRepository<FeaturedDeal>
{
    /// <summary>
    /// Retrieves all featured deals with hotel and city details.
    /// </summary>
    /// <returns>A collection of FeaturedDeal with hotel and city details.</returns>
    Task<IEnumerable<FeaturedDeal>> GetAllWithHotelAndCityAsync();

    /// <summary>
    /// Retrieves a featured deal by its Id with hotel and city details.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>A FeaturedDeal with hotel and city details, null if not found.</returns>
    Task<FeaturedDeal?> GetByIdWithHotelAndCityAsync(int id);
}
