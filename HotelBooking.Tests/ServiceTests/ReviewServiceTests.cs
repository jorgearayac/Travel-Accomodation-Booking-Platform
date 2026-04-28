using HotelBooking.API.DTOs.Reviews;
using HotelBooking.API.Services;
using HotelBooking.Db.Enums;
using HotelBooking.Db.Interfaces;
using HotelBooking.Db.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBooking.Tests.ServiceTests;

public class ReviewServiceTests
{
    private readonly Mock<IReviewRepository> _mockReviewRepo;
    private readonly ReviewService _reviewService;

    public ReviewServiceTests()
    {
        _mockReviewRepo = new Mock<IReviewRepository>();
        _reviewService = new ReviewService(_mockReviewRepo.Object);
    }

    // CreateReviewAsync tests
    [Fact]
    public async Task CreateReviewAsync_WithValidData_ReturnsReviewResponse()
    {
        // Arrange
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
        Assert.NotNull(result);
        Assert.Equal(4, result.Rating);
        Assert.Equal("Great hotel!", result.Comment);
        Assert.Equal("testuser", result.Username);
        _mockReviewRepo
            .Verify(r => r.AddAsync(It.IsAny<Review>()), Times.Once);
    }

    [Fact]
    public async Task CreateReviewAsync_WithDuplicateReview_ThrowsInvalidOperationException()
    {
        // Arrange — user already reviewed this hotel
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

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _reviewService.CreateReviewAsync(1, request));

        _mockReviewRepo.Verify(r => r.AddAsync(It.IsAny<Review>()), Times.Never);
    }
    // DeleteReviewAsync tests
    [Fact]
    public async Task DeleteReviewAsync_WithOwnReview_Succeeds()
    {
        // Arrange
        var review = CreateTestReview(1, 1, 1);
        _mockReviewRepo
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(review);

        // Act
        await _reviewService.DeleteReviewAsync(1, 1);

        // Assert
        _mockReviewRepo.Verify(r => r.DeleteAsync(review), Times.Once);
    }

    [Fact]
    public async Task DeleteReviewAsync_WithNonExistentReview_ThrowsKeyNotFoundException()
    {
        // Arrange
        _mockReviewRepo
            .Setup(r => r.GetByIdAsync(67))
            .ReturnsAsync((Review?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _reviewService.DeleteReviewAsync(1, 67));
    }

    [Fact]
    public async Task DeleteReviewAsync_WithOtherUsersReview_ThrowsUnauthorizedAccessException()
    {
        // Arrange — review belongs to user 1, but user 2 tries to delete
        var review = CreateTestReview(1, 1, 1);
        _mockReviewRepo
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(review);

        // Act & Assert — user 2 trying to delete user 1's review
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _reviewService.DeleteReviewAsync(2, 1));

        _mockReviewRepo.Verify(r => r.DeleteAsync(It.IsAny<Review>()), Times.Never);
    }

    // GetReviewsByHotelIdAsync tests
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
        Assert.Equal(2, result.Count());
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
        Assert.Empty(result);
    }

    // Helper methods for creating test data
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
