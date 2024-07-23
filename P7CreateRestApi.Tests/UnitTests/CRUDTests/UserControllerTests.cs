using System.Security.Claims;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using P7CreateRestApi.Controllers;

namespace P7CreateRestApi.Tests;

public class UserControllerTests : TestBase<User>
{
    private readonly UserController _controller;
    private readonly Mock<UserManager<User>> _mockUserManager;
    private readonly Mock<IJwtRevocationService> _mockJwtRevocationService;

    public UserControllerTests()
    {
        _mockUserManager = MockUserManager<User>();
        _mockJwtRevocationService = new Mock<IJwtRevocationService>();

        ServiceCollection services = new();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddAuthentication();
        ServiceProvider serviceProvider = services.BuildServiceProvider();
        IHttpContextAccessor httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();

        _controller = new UserController(_context, _mockUserManager.Object, _mockJwtRevocationService.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { RequestServices = serviceProvider }
            }
        };
    }

    [Fact]
    public async Task GetUsers_ShouldReturnUsers()
    {
        // Arrange
        List<User> users = new()
        {
            new User { Id = "1", UserName = "userone", Fullname = "User One", Email = "userone@example.com" },
            new User { Id = "2", UserName = "usertwo", Fullname = "User Two", Email = "usertwo@example.com" }
        };
        _context.Users.AddRange(users);
        await _context.SaveChangesAsync();

        // Act
        IActionResult result = await _controller.GetUsers();

        // Assert
        OkObjectResult actionResult = Assert.IsType<OkObjectResult>(result);
        List<User> returnValue = Assert.IsType<List<User>>(actionResult.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task GetUser_ExistingId_ShouldReturnUser()
    {
        // Arrange
        User user = new() { Id = "1", UserName = "userone", Fullname = "User One", Email = "userone@example.com" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        ActionResult<User> result = await _controller.GetUser("1");

        // Assert
        ActionResult<User> actionResult = Assert.IsType<ActionResult<User>>(result);
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        User returnValue = Assert.IsType<User>(okResult.Value);
        Assert.Equal(user, returnValue);
    }

    [Fact]
    public async Task PutUser_ValidUpdate_ShouldReturnNoContent()
    {
        // Arrange
        User user = new() { Id = "1", UserName = "userone", Fullname = "User One", Email = "userone@example.com" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        User updatedUser = new() { Id = "1", UserName = "userone", Fullname = "Updated User", Email = "updateduser@example.com" };

        _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
        _mockUserManager.Setup(um => um.IsInRoleAsync(user, "Admin")).ReturnsAsync(true);
        _mockUserManager.Setup(um => um.FindByIdAsync(user.Id)).ReturnsAsync(user);
        _mockUserManager.Setup(um => um.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);

        // Act
        IActionResult result = await _controller.PutUser("1", updatedUser);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        User? foundUser = await _context.Users.FindAsync("1");
        if (foundUser == null)
        {
            Assert.Fail("User updated not found");
        }
        Assert.Equal(updatedUser.Fullname, foundUser.Fullname);
        Assert.Equal(updatedUser.Email, foundUser.Email);
    }

    [Theory]
    [InlineData("1", true, false, typeof(NoContentResult))]
    [InlineData("2", false, false, typeof(ForbidResult))]
    [InlineData("1", false, true, typeof(NoContentResult))]
    public async Task DeleteUser_ShouldReturnExpectedResult(string userIdToDelete, bool isAdmin, bool isActiveUser, Type expectedType)
    {
        // Arrange
        User user = new() { Id = "1", UserName = "userName", Fullname = "User Name", Email = "username@example.com" };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        if (userIdToDelete != "1")
        {
            User userToDelete = new() { Id = userIdToDelete, UserName = "userNameToDelete", Fullname = "User NameToDelete", Email = "usernametodelete@example.com" };
            _context.Users.Add(userToDelete);
            await _context.SaveChangesAsync();
        }

        await _context.SaveChangesAsync();

        SetupMockAuthServices(user, isAdmin);

        if (isActiveUser)
        {
            SetupMockActiveUser(user);
        }

        // Reload the user to ensure the context is aware of the current state of the entity
        _context.Entry(user).Reload();
        if (userIdToDelete != "1")
        {
            User? userToDelete = await _context.Users.FindAsync(userIdToDelete);
            _context.Entry(userToDelete).Reload();
        }

        // Act
        IActionResult result = await _controller.DeleteUser(userIdToDelete);

        // Assert
        Assert.IsType(expectedType, result);
    }

    [Fact]
    public async Task DeleteUser_NonExistingId_ShouldReturnNotFound()
    {
        // Arrange
        User user = new() { Id = "1", UserName = "userone", Fullname = "User One", Email = "userone@example.com" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        SetupMockAuthServices(user, true);

        // Act
        IActionResult result = await _controller.DeleteUser("99");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    private void SetupMockAuthServices(User user, bool isAdmin)
    {
        List<Claim> userClaims =
        [
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName)
        ];

        if (isAdmin)
        {
            userClaims.Add(new Claim(ClaimTypes.Role, "Admin"));
        }

        ClaimsIdentity identity = new(userClaims, "TestAuthType");
        ClaimsPrincipal claimsPrincipal = new(identity);

        Mock<IAuthenticationService> mockAuthService = new();
        mockAuthService.Setup(x => x.SignOutAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<AuthenticationProperties>())).Returns(Task.CompletedTask);

        Mock<IServiceProvider> mockServiceProvider = new();
        mockServiceProvider.Setup(x => x.GetService(typeof(IAuthenticationService))).Returns(mockAuthService.Object);

        DefaultHttpContext httpContext = new()
        {
            User = claimsPrincipal,
            RequestServices = mockServiceProvider.Object
        };
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
        _mockUserManager.Setup(um => um.IsInRoleAsync(It.IsAny<User>(), "Admin")).ReturnsAsync(isAdmin);

        _mockJwtRevocationService.Setup(j => j.RevokeUserTokensAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
    }

    private void SetupMockActiveUser(User user)
    {
        List<Claim> userClaims = new()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName)
        };

        ClaimsIdentity identity = new(userClaims, "TestAuthType");
        ClaimsPrincipal claimsPrincipal = new(identity);

        Mock<IAuthenticationService> mockAuthService = new();
        mockAuthService.Setup(x => x.SignOutAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<AuthenticationProperties>())).Returns(Task.CompletedTask);

        Mock<IServiceProvider> mockServiceProvider = new();
        mockServiceProvider.Setup(x => x.GetService(typeof(IAuthenticationService))).Returns(mockAuthService.Object);

        DefaultHttpContext httpContext = new()
        {
            User = claimsPrincipal,
            RequestServices = mockServiceProvider.Object
        };
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
    }
}