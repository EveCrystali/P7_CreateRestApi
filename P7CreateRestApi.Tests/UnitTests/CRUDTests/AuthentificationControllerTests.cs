using System.Security.Claims;
using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Models;
using Dot.Net.WebApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace P7CreateRestApi.Tests
{
    public class AuthentificationControllerTests : TestBase<User>
    {
        private readonly AuthentificationController _controller;
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<IJwtService> _mockJwtService;

        public AuthentificationControllerTests()
        {
            _mockUserManager = MockUserManager<User>();
            _mockJwtService = new Mock<IJwtService>();

            _controller = new AuthentificationController(_mockUserManager.Object, _mockJwtService.Object, _context);
        }

        private Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
        {
            Mock<IUserStore<TUser>> store = new();
            return new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
        }

        private ClaimsPrincipal CreateClaimsPrincipal(string userId, string userName, string role)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, role)
            };
            ClaimsIdentity identity = new(claims, "TestAuthType");
            return new ClaimsPrincipal(identity);
        }

        [Fact]
        public async Task Login_ValidCredentials_ShouldReturnTokenAndRefreshToken()
        {
            // Arrange
            User user = new() { Id = "1", UserName = "testuser", Fullname = "Test User", Email = "testuser@example.com" };
            LoginModel loginModel = new() { Username = "testuser", Password = "Password123" };

            _mockUserManager.Setup(um => um.FindByNameAsync(loginModel.Username)).ReturnsAsync(user);
            _mockUserManager.Setup(um => um.CheckPasswordAsync(user, loginModel.Password)).ReturnsAsync(true);
            _mockUserManager.Setup(um => um.GetRolesAsync(user)).ReturnsAsync(new List<string> { "User" });
            _mockJwtService.Setup(js => js.GenerateToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string[]>())).Returns("token");
            _mockJwtService.Setup(js => js.GenerateRefreshToken(It.IsAny<string>())).Returns(new RefreshToken { Token = "refreshToken", UserId = user.Id });

            // Act
            IActionResult result = await _controller.Login(loginModel);

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            object? returnValue = okResult.Value;

            System.Reflection.PropertyInfo? tokenProperty = returnValue.GetType().GetProperty("Token");
            System.Reflection.PropertyInfo? refreshTokenProperty = returnValue.GetType().GetProperty("RefreshToken");

            Assert.NotNull(tokenProperty);
            Assert.NotNull(refreshTokenProperty);

            string? tokenValue = tokenProperty.GetValue(returnValue).ToString();
            string? refreshTokenValue = refreshTokenProperty.GetValue(returnValue).ToString();

            Assert.Equal("token", tokenValue);
            Assert.Equal("refreshToken", refreshTokenValue);
        }

        [Fact]
        public async Task Register_ValidModel_ShouldReturnSuccessMessage()
        {
            // Arrange
            RegisterModel registerModel = new() { Username = "testuser", Fullname = "Test User", Password = "Password123" };
            _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<User>(), registerModel.Password)).ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(um => um.AddToRoleAsync(It.IsAny<User>(), "User")).ReturnsAsync(IdentityResult.Success);

            // Act
            IActionResult result = await _controller.Register(registerModel);

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            object? returnValue = okResult.Value;

            System.Reflection.PropertyInfo? messageProperty = returnValue.GetType().GetProperty("Message");
            Assert.NotNull(messageProperty);

            string? messageValue = messageProperty.GetValue(returnValue).ToString();
            Assert.Equal("User registered successfully", messageValue);
        }

        [Fact]
        public async Task Refresh_ValidRefreshToken_ShouldReturnNewTokenAndRefreshToken()
        {
            // Arrange
            User user = new() { Id = "1", UserName = "testuser", Fullname = "Test User", Email = "testuser@example.com", LastLoginDate = DateTime.UtcNow };
            RefreshRequest refreshRequest = new() { RefreshToken = "oldRefreshToken" };
            RefreshToken oldRefreshToken = new()
            {
                Token = "oldRefreshToken",
                UserId = "1",
                ExpiryDate = DateTime.UtcNow.AddMinutes(10),
                IsRevoked = false
            };

            _context.RefreshTokens.Add(oldRefreshToken);
            await _context.SaveChangesAsync();

            _mockUserManager.Setup(um => um.FindByIdAsync("1")).ReturnsAsync(user);
            _mockUserManager.Setup(um => um.GetRolesAsync(user)).ReturnsAsync(new List<string> { "User" });

            _mockJwtService.Setup(js => js.GenerateToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string[]>())).Returns("newToken");
            _mockJwtService.Setup(js => js.GenerateRefreshToken(It.IsAny<string>())).Returns(new RefreshToken { Token = "newRefreshToken", UserId = user.Id });

            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(user.Id, user.UserName, "User");
            SetupControllerContext(_controller, claimsPrincipal);

            IActionResult result = await _controller.Refresh(refreshRequest);

            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            object? returnValue = okResult.Value;

            System.Reflection.PropertyInfo? tokenProperty = returnValue.GetType().GetProperty("Token");
            System.Reflection.PropertyInfo? refreshTokenProperty = returnValue.GetType().GetProperty("RefreshToken");

            Assert.NotNull(tokenProperty);
            Assert.NotNull(refreshTokenProperty);

            string? tokenValue = tokenProperty.GetValue(returnValue).ToString();
            string? refreshTokenValue = refreshTokenProperty.GetValue(returnValue).ToString();

            Assert.Equal("newToken", tokenValue);
            Assert.Equal("newRefreshToken", refreshTokenValue);
        }

        [Fact]
        public async Task RevokeAllTokens_ValidUser_ShouldRevokeTokens()
        {
            // Arrange
            User user = new() { Id = "1", UserName = "testuser", Fullname = "Test User", Email = "testuser@example.com" };
            RevokeTokensRequest revokeRequest = new() { UserId = "1" };
            List<RefreshToken> refreshTokens = new()
            {
                new RefreshToken { Token = "token1", UserId = "1", IsRevoked = false, ExpiryDate = DateTime.UtcNow.AddMinutes(5) },
                new RefreshToken { Token = "token2", UserId = "1", IsRevoked = false, ExpiryDate = DateTime.UtcNow.AddMinutes(5) }
            };

            _context.RefreshTokens.AddRange(refreshTokens);
            await _context.SaveChangesAsync();

            _mockUserManager.Setup(um => um.FindByIdAsync("1")).ReturnsAsync(user);

            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal("adminId", "admin", "Admin");
            SetupControllerContext(_controller, claimsPrincipal);

            //Act
            IActionResult result = await _controller.RevokeTokens(revokeRequest);

            // Assert
            Assert.IsType<OkResult>(result);
            List<RefreshToken> tokens = await _context.RefreshTokens.ToListAsync();
            Assert.All(tokens, t => Assert.True(t.IsRevoked));
        }
    }
}