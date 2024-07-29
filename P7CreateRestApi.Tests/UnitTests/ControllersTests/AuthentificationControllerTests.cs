using System.Security.Claims;
using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Models;
using Dot.Net.WebApi.Services;
using Microsoft.AspNetCore.Http;
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

        private void SetupControllerContext(Controller controller, ClaimsPrincipal user)
        {
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };
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
            RegisterModel registerModel = new() { Username = "testuser", Email = "testuser@example.com", Fullname = "Test User", Password = "Password123" };
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

        /// <summary>
        /// Test case for the Logout method when the user is valid.
        /// It should return an OkResult with a message indicating the logout was successful.
        /// It should also revoke the refresh token of the user.
        /// </summary>
        [Fact]
        public async Task Logout_ValidUser_ShouldReturnOkResult()
        {
            // Arrange
            // Create a test user
            string userId = "test-user-id";
            RefreshToken refreshToken = new()
            {
                Token = "token1",
                UserId = userId,
                IsRevoked = false,
                ExpiryDate = DateTime.UtcNow.AddMinutes(5)
            };

            // Add the refresh token to the test context
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            // Create a test claims principal with the user's information
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(userId, "testuser", "User");

            // Setup the controller context with the test claims principal
            SetupControllerContext(_controller, claimsPrincipal);

            // Act
            IActionResult result = await _controller.Logout();

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);


            object returnValue = okResult.Value;

            // Get the PropertyInfo of the "Message" property of the returnValue object
            System.Reflection.PropertyInfo messageProperty = returnValue.GetType().GetProperty("Message");

            // Assert that the messageProperty is not null
            Assert.NotNull(messageProperty);

            // Get the value of the "Message" property and assert that it is equal to "Logout successful"
            string messageValue = messageProperty.GetValue(returnValue).ToString();
            Assert.Equal("Logout successful", messageValue);

            // Find the revoked refresh token in the test context
            RefreshToken revokedToken = await _context.RefreshTokens.FindAsync(refreshToken.Id);

            // Assert that the revoked token is revoked
            Assert.True(revokedToken.IsRevoked);
        }

        [Fact]
        public async Task Logout_UserIdNotFound_ShouldReturnBadRequest()
        {
            // Arrange
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
            SetupControllerContext(_controller, claimsPrincipal);

            // Act
            IActionResult result = await _controller.Logout();

            // Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Impossible to get user ID.", badRequestResult.Value);
        }

        [Fact]
        public async Task Logout_RefreshTokenNotFound_ShouldReturnBadRequest()
        {
            // Arrange
            string userId = "test-user-id";

            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(userId, "testuser", "User");
            SetupControllerContext(_controller, claimsPrincipal);

            // Act
            IActionResult result = await _controller.Logout();

            // Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Impossible to get current token.", badRequestResult.Value);
        }
    }
}