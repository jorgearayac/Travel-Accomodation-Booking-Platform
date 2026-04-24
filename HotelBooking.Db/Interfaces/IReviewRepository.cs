using HotelBooking.Db.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBooking.Db.Interfaces;

public interface IReviewRepository : IRepository<Review>
{
    // Get all reviews for a specific hotel
    Task<IEnumerable<Review>> GetByHotelIdAsync(int hotelId);

    // Get a review by its Id along with the user who created it
    Task<Review?> GetByIdWithUserAsync(int id);

    // Get a review by its user Id and hotel Id (to check if a user has already reviewed a hotel)
    Task<Review?> GetByUserAndHotelAsync(int userId, int hotelId);
}
