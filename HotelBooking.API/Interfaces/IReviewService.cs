using HotelBooking.API.Common;
using HotelBooking.API.DTOs.Reviews;

namespace HotelBooking.API.Interfaces;

public interface IReviewService
{
    Task<Result<IEnumerable<ReviewResponse>>> GetReviewsByHotelIdAsync(int hotelId);
    Task<Result<ReviewResponse>> CreateReviewAsync(int userId, CreateReviewRequest request);
    Task<Result> DeleteReviewAsync(int userId, int reviewId);
}
