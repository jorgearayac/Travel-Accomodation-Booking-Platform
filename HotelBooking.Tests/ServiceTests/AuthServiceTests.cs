using HotelBooking.API.DTOs.Auth;
using HotelBooking.API.Interfaces;
using HotelBooking.API.Services;
using HotelBooking.Db.Enums;
using HotelBooking.Db.Interfaces;
using HotelBooking.Db.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace HotelBooking.Tests.ServiceTests;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly Mock<ITokenService> _mockTokenService;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _mockUserRepo = new Mock<IUserRepository>();
        _mockTokenService = new Mock<ITokenService>();
        var mockLogger = new Mock<ILogger<AuthService>>();
        _authService = new AuthService(_mockUserRepo.Object, _mockTokenService.Object, mockLogger.Object);
    }

    [Fact]
    public async Task LoginUserAsync_WithValidCredentials_ReturnsAuthResponse()
    {
        // Arrange
        var user = CreateTestUser("jorge", "TestPassword");

        _mockUserRepo
            .Setup(r => r.GetByUsernameAsync("jorge"))
            .ReturnsAsync(user);

        _mockTokenService
            .Setup(t => t.GenerateToken(user))
            .Returns("fake-jwt-token");

        // Act
        var result = await _authService.LoginUserAsync(
            new LoginRequest { Username = "jorge", Password = "TestPassword" });

        // Assert
        Assert.NotNull(result);
        Assert.Equal("jorge", result.Username);
        Assert.Equal("fake-jwt-token", result.Token);
    }

    [Fact]
    public async Task LoginUserAsync_WithInvalidPassword_ThrowsUnauthorized()
    {
        // Arrange
        var user = CreateTestUser("jorge", "CorrectPassword");

        _mockUserRepo
            .Setup(r => r.GetByUsernameAsync("jorge"))
            .ReturnsAsync(user);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _authService.LoginUserAsync(
                new LoginRequest { Username = "jorge", Password = "WrongPassword" }));
    }

    [Fact]
    public async Task LoginUserAsync_WithNonExistentUser_ThrowsUnauthorized()
    {
        // Arrange - GetByUsernameAsync returns null for non-existent user
        _mockUserRepo
            .Setup(r => r.GetByUsernameAsync("nonexistent"))
            .ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _authService.LoginUserAsync(
                new LoginRequest { Username = "nonexistent", Password = "AnyPassword" }));
    }

    [Fact]
    public async Task RegisterUserAsync_WithNewUser_ReturnsAuthResponse()
    {
        // Arrange
        _mockUserRepo
            .Setup(r => r.GetByUsernameAsync("newuser"))
            .ReturnsAsync((User?)null); // No existing user

        _mockUserRepo
            .Setup(r => r.GetByEmailAsync("newuser@example.com"))
            .ReturnsAsync((User?)null);

        _mockUserRepo
            .Setup(r => r.AddAsync(It.IsAny<User>()))
            .ReturnsAsync((User u) => u); // Return the user that was added

        _mockTokenService
            .Setup(t => t.GenerateToken(It.IsAny<User>()))
            .Returns("new-user-jwt-token");

        // Act
        var request = new RegisterRequest
        {
            Username = "newuser",
            Password = "NewUserPassword",
            Email = "newuser@example.com",
            FirstName = "New",
            LastName = "User"
        };
        var result = await _authService.RegisterUserAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("newuser", result.Username);
        Assert.Equal("new-user-jwt-token", result.Token);
        Assert.Equal("User", result.Role);
        _mockUserRepo
            .Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task RegisterUserAsync_WithExistingUsername_ThrowsArgumentException()
    {
        // Arrange
        var existingUser = CreateTestUser("existingUser", "TestPassword");
        _mockUserRepo
            .Setup(r => r.GetByUsernameAsync("existingUser"))
            .ReturnsAsync(existingUser);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _authService.RegisterUserAsync(new RegisterRequest
            {
                Username = "existingUser",
                Password = "AnyPassword",
                Email = "new@example.com",
                FirstName = "Existing",
                LastName = "User"
            }));

        Assert.Contains("Username", exception.Message);
        _mockUserRepo
            .Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task RegisterUserAsync_WithExistingEmail_ThrowsArgumentException()
    {
        // Arrange
        _mockUserRepo
            .Setup(r => r.GetByUsernameAsync("newuser"))
            .ReturnsAsync((User?)null); // username is available

        var existingUser = CreateTestUser("anotherUser", "TestPassword");

        _mockUserRepo
            .Setup(r => r.GetByEmailAsync("existing@example.com"))
            .ReturnsAsync(existingUser); // email is already in use

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _authService.RegisterUserAsync(new RegisterRequest
            {
                Username = "newuser",
                Password = "TestPassword",
                Email = "existing@example.com",
                FirstName = "New",
                LastName = "User"
            }));

        Assert.Contains("Email", exception.Message);
        _mockUserRepo
            .Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
    }

    private static User CreateTestUser(string username, string password)
    {
        return new User
        {
            Id = 1,
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            FirstName = "Test",
            LastName = "User",
            Email = $"{username}@example.com",
            Role = UserRole.User
        };
    }
}
