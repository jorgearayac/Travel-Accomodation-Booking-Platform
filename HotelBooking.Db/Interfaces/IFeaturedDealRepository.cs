using HotelBooking.Db.Models;

namespace HotelBooking.Db.Interfaces;

public interface IFeaturedDealRepository : IRepository<FeaturedDeal>
{
    Task<IEnumerable<FeaturedDeal>> GetAllWithHotelAndCityAsync();

    Task<FeaturedDeal?> GetByIdWithHotelAndCityAsync(int id);
}
