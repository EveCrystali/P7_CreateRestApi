using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Dot.Net.WebApi.Models;
using Dot.Net.WebApi.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Xunit;

namespace P7CreateRestApi.Tests;

public class JwtServiceTests
{
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly JwtService _jwtService;

    public JwtServiceTests()
    {
        _mockConfiguration = new Mock<IConfiguration>();
        _jwtService = new JwtService(_mockConfiguration.Object);
    }

    private void SetupConfiguration(string tokenLifetime, string refreshTokenLifetime, string issuer, string audience, string secretKey)
    {
        _mockConfiguration.Setup(c => c["Jwt:TokenLifetimeMinutes"]).Returns(tokenLifetime);
        _mockConfiguration.Setup(c => c["Jwt:RefreshTokenLifetimeDays"]).Returns(refreshTokenLifetime);
        _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns(issuer);
        _mockConfiguration.Setup(c => c["Jwt:Audience"]).Returns(audience);

        Environment.SetEnvironmentVariable("JWT_SECRET_KEY", secretKey);
    }

    /// <summary>
    /// Tests the GenerateToken method with valid inputs and expects it to return a JWT token.
    /// </summary>
    [Fact]
    public void GenerateToken_ValidInputs_ReturnsJwtToken()
    {
        // Arrange
        string userId = "test-user-id";
        string userName = "testuser";
        string[] roles = { "User", "Admin" };

        // Set up the configuration for the JwtService.
        SetupConfiguration("60", "7", "testIssuer", "testAudience", "supersecretkeythatneedstobe32byteslong");

        // Act
        string token = _jwtService.GenerateToken(userId, userName, roles);

        // Assert
        Assert.NotNull(token);

        // Create a JwtSecurityTokenHandler to read the token.
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        // Read the token to get the claims.
        JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(token);

        // Check that the claims in the token have the expected values.
        Assert.Equal(userId, jwtToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
        Assert.Equal(userName, jwtToken.Claims.First(c => c.Type == ClaimTypes.Name).Value);
        Assert.Contains(jwtToken.Claims, c => c.Type == ClaimTypes.Role && c.Value == "User");
        Assert.Contains(jwtToken.Claims, c => c.Type == ClaimTypes.Role && c.Value == "Admin");

        // Check that the issuer and audience in the token have the expected values.
        Assert.Equal("testIssuer", jwtToken.Issuer);
        Assert.Equal("testAudience", jwtToken.Audiences.First());
    }

    [Fact]
    public void GenerateToken_MissingSecretKey_ThrowsInvalidOperationException()
    {
        // Arrange
        string userId = "test-user-id";
        string userName = "testuser";
        string[] roles = { "User" };
        SetupConfiguration("60", "7", "testIssuer", "testAudience", "");

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => _jwtService.GenerateToken(userId, userName, roles));
        Assert.Equal("The JWT secret key is not defined in the environment variables", exception.Message);
    }

    /// <summary>
    /// Tests the GenerateRefreshToken method with a valid user ID and expects it to return a refresh token.
    /// </summary>
    [Fact]
    public void GenerateRefreshToken_ValidUserId_ReturnsRefreshToken()
    {
        // Arrange
        string userId = "test-user-id";

        // Set up the configuration for the JwtService.
        SetupConfiguration("60", "7", "testIssuer", "testAudience", "supersecretkeythatneedstobe32byteslong");

        // Act
        // Generate a refresh token using the user ID.
        RefreshToken refreshToken = _jwtService.GenerateRefreshToken(userId);

        // Assert
        Assert.NotNull(refreshToken);

        // Check that the user ID in the refresh token matches the test user ID.
        Assert.Equal(userId, refreshToken.UserId);

        // Check that the IsRevoked property is false.
        Assert.False(refreshToken.IsRevoked);

        // Check that the ExpiryDate property is after the current date and time.
        Assert.True(refreshToken.ExpiryDate > DateTime.UtcNow);

        // Check that the token in the refresh token is not null or empty.
        Assert.False(string.IsNullOrEmpty(refreshToken.Token));
    }
}
