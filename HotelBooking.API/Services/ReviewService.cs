using HotelBooking.API.DTOs.Reviews;
using HotelBooking.API.Interfaces;
using HotelBooking.Db.Interfaces;
using HotelBooking.Db.Models;

namespace HotelBooking.API.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IHotelRepository _hotelRepository;
    private readonly ILogger<ReviewService> _logger;

    public ReviewService(
        IReviewRepository reviewRepository,
        IHotelRepository hotelRepository,
        ILogger<ReviewService> logger)
    {
        _reviewRepository = reviewRepository;
        _hotelRepository = hotelRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<ReviewResponse>> GetReviewsByHotelIdAsync(int hotelId)
    {
        var reviews = await _reviewRepository.GetByHotelIdAsync(hotelId);
        return reviews.Select(MapToResponse);
    }

    public async Task<ReviewResponse> CreateReviewAsync(int userId, CreateReviewRequest request)
    {
        var hotel = await _hotelRepository.GetByIdAsync(request.HotelId);
        if (hotel == null)
        {
            throw new KeyNotFoundException($"Hotel with Id {request.HotelId} not found.");
        }

        var existingReview = await _reviewRepository.GetByUserAndHotelAsync(userId, request.HotelId);
        if (existingReview != null)
        {
            throw new InvalidOperationException("You have already reviewed this hotel.");
        }

        var review = new Review
        {
            UserId = userId,
            HotelId = request.HotelId,
            Rating = request.Rating,
            Comment = request.Comment,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow,
        };

        await _reviewRepository.AddAsync(review);
        _logger.LogInformation("Review created by user {UserId} for hotel {HotelId}", userId, request.HotelId);

        var createdReview = await _reviewRepository.GetByIdWithUserAsync(review.Id);
        return MapToResponse(createdReview!);
    }

    public async Task DeleteReviewAsync(int userId, int reviewId)
    {
        var review = await _reviewRepository.GetByIdWithUserAsync(reviewId);
        if (review == null)
        {
            throw new KeyNotFoundException($"Review with id {reviewId} not found.");
        }

        if (review.UserId != userId)
        {
            throw new UnauthorizedAccessException("You can only delete your own reviews.");
        }

        await _reviewRepository.DeleteAsync(review);
        _logger.LogInformation("Review {ReviewId} deleted by user {UserId}", reviewId, userId);
    }

    private ReviewResponse MapToResponse(Review review)
    {
        return new ReviewResponse
        {
            Id = review.Id,
            UserId = review.UserId,
            Username = review.User?.Username ?? "Unknown User",
            HotelId = review.HotelId,
            HotelName = review.Hotel?.Name ?? "Unknown Hotel",
            Rating = review.Rating,
            Comment = review.Comment,
            CreatedDate = review.CreatedDate
        };
    }
}
