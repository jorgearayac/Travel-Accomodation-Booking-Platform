using HotelBooking.API.Common;
using HotelBooking.API.DTOs.Reviews;
using HotelBooking.API.Services;
using HotelBooking.Db.Enums;
using HotelBooking.Db.Interfaces;
using HotelBooking.Db.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace HotelBooking.Tests.ServiceTests;

public class ReviewServiceTests
{
    private readonly Mock<IReviewRepository> _mockReviewRepo;
    private readonly Mock<IHotelRepository> _mockHotelRepo;
    private readonly ReviewService _reviewService;

    public ReviewServiceTests()
    {
        _mockReviewRepo = new Mock<IReviewRepository>();
        _mockHotelRepo = new Mock<IHotelRepository>();
        var mockLogger = new Mock<ILogger<ReviewService>>();
        _reviewService = new ReviewService(_mockReviewRepo.Object, _mockHotelRepo.Object, mockLogger.Object);
    }

    [Fact]
    public async Task CreateReviewAsync_WithValidData_ReturnsReviewResponse()
    {
        // Arrange
        _mockHotelRepo
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Hotel
            {
                Id = 1, Name = "Test Hotel", StarRate = 4, Owner = "Owner",
                Description = "Desc", PricePerNight = 100m, ThumbnailUrl = "url", CityId = 1
            });

        _mockReviewRepo
            .Setup(r => r.GetByUserAndHotelAsync(1, 1))
            .ReturnsAsync((Review?)null);

        _mockReviewRepo
            .Setup(r => r.AddAsync(It.IsAny<Review>()))
            .ReturnsAsync((Review r) => r);

        var testReview = CreateTestReview(1, 1, 1);
        _mockReviewRepo
            .Setup(r => r.GetByIdWithUserAsync(It.IsAny<int>()))
            .ReturnsAsync(testReview);

        var request = new CreateReviewRequest
        {
            HotelId = 1,
            Rating = 4,
            Comment = "Great hotel!"
        };

        // Act
        var result = await _reviewService.CreateReviewAsync(1, request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(4, result.Value!.Rating);
        Assert.Equal("Great hotel!", result.Value.Comment);
        Assert.Equal("testuser", result.Value.Username);
        _mockReviewRepo
            .Verify(r => r.AddAsync(It.IsAny<Review>()), Times.Once);
    }

    [Fact]
    public async Task CreateReviewAsync_WithNonExistentHotel_ReturnsNotFound()
    {
        // Arrange
        _mockHotelRepo
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((Hotel?)null);

        var request = new CreateReviewRequest
        {
            HotelId = 999,
            Rating = 4,
            Comment = "Review for non-existent hotel"
        };

        // Act
        var result = await _reviewService.CreateReviewAsync(1, request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.NotFound, result.Error!.Type);
        _mockReviewRepo.Verify(r => r.AddAsync(It.IsAny<Review>()), Times.Never);
    }

    [Fact]
    public async Task CreateReviewAsync_WithDuplicateReview_ReturnsConflictError()
    {
        // Arrange — user already reviewed this hotel
        _mockHotelRepo
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Hotel
            {
                Id = 1, Name = "Test Hotel", StarRate = 4, Owner = "Owner",
                Description = "Desc", PricePerNight = 100m, ThumbnailUrl = "url", CityId = 1
            });

        var existingReview = CreateTestReview(1, 1, 1);
        _mockReviewRepo
            .Setup(r => r.GetByUserAndHotelAsync(1, 1))
            .ReturnsAsync(existingReview);

        var request = new CreateReviewRequest
        {
            HotelId = 1,
            Rating = 5,
            Comment = "Trying to review again"
        };

        // Act
        var result = await _reviewService.CreateReviewAsync(1, request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Conflict, result.Error!.Type);
        _mockReviewRepo.Verify(r => r.AddAsync(It.IsAny<Review>()), Times.Never);
    }

    [Fact]
    public async Task DeleteReviewAsync_WithOwnReview_Succeeds()
    {
        // Arrange
        var review = CreateTestReview(1, 1, 1);
        _mockReviewRepo
            .Setup(r => r.GetByIdWithUserAsync(1))
            .ReturnsAsync(review);

        // Act
        var result = await _reviewService.DeleteReviewAsync(1, 1);

        // Assert
        Assert.True(result.IsSuccess);
        _mockReviewRepo.Verify(r => r.DeleteAsync(review), Times.Once);
    }

    [Fact]
    public async Task DeleteReviewAsync_WithNonExistentReview_ReturnsNotFound()
    {
        // Arrange
        _mockReviewRepo
            .Setup(r => r.GetByIdWithUserAsync(67))
            .ReturnsAsync((Review?)null);

        // Act
        var result = await _reviewService.DeleteReviewAsync(1, 67);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.NotFound, result.Error!.Type);
    }

    [Fact]
    public async Task DeleteReviewAsync_WithOtherUsersReview_ReturnsUnauthorized()
    {
        // Arrange — review belongs to user 1, but user 2 tries to delete
        var review = CreateTestReview(1, 1, 1);
        _mockReviewRepo
            .Setup(r => r.GetByIdWithUserAsync(1))
            .ReturnsAsync(review);

        // Act — user 2 trying to delete user 1's review
        var result = await _reviewService.DeleteReviewAsync(2, 1);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Unauthorized, result.Error!.Type);
        _mockReviewRepo.Verify(r => r.DeleteAsync(It.IsAny<Review>()), Times.Never);
    }

    [Fact]
    public async Task GetReviewsByHotelIdAsync_WithReviews_ReturnsReviewList()
    {
        // Arrange
        var reviews = new List<Review>
        {
            CreateTestReview(1, 1, 1),
            CreateTestReview(2, 2, 1)
        };
        _mockReviewRepo
            .Setup(r => r.GetByHotelIdAsync(1))
            .ReturnsAsync(reviews);

        // Act
        var result = await _reviewService.GetReviewsByHotelIdAsync(1);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value!.Count());
    }

    [Fact]
    public async Task GetReviewsByHotelIdAsync_WithNoReviews_ReturnsEmptyList()
    {
        // Arrange
        _mockReviewRepo
            .Setup(r => r.GetByHotelIdAsync(67))
            .ReturnsAsync(new List<Review>());

        // Act
        var result = await _reviewService.GetReviewsByHotelIdAsync(67);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value!);
    }

    private Review CreateTestReview(int id, int userId, int hotelId)
    {
        return new Review
        {
            Id = id,
            UserId = userId,
            HotelId = hotelId,
            Rating = 4,
            Comment = "Great hotel!",
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow,
            User = new User
            {
                Id = userId,
                Username = "testuser",
                PasswordHash = "hash",
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                Role = UserRole.User
            },
            Hotel = new Hotel
            {
                Id = hotelId,
                Name = "Test Hotel",
                StarRate = 4,
                Owner = "Owner",
                Description = "Description",
                PricePerNight = 100m,
                ThumbnailUrl = "https://example.com/hotel.jpg",
                CityId = 1
            }
        };
    }
}
