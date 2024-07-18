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
                HttpContext = new DefaultHttpContext
                {
                    RequestServices = serviceProvider
                }
            }
        };
    }

    private Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
    {
        Mock<IUserStore<TUser>> store = new();
        return new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
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

        // Simulate an HTTP context with a valid user
        List<Claim> userClaims = new()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, "Admin")
         };
        ClaimsIdentity identity = new(userClaims, "TestAuthType");
        ClaimsPrincipal claimsPrincipal = new(identity);

        DefaultHttpContext httpContext = new()
        {
            User = claimsPrincipal
        };
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        // Act
        IActionResult result = await _controller.PutUser("1", updatedUser);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task PostUser_ValidData_ShouldReturnCreatedAtAction()
    {
        // Arrange
        User user = new() { UserName = "newuser", Fullname = "New User", Email = "newuser@example.com", PasswordHash = "password" };

        _mockUserManager.Setup(um => um.CreateAsync(user, user.PasswordHash)).ReturnsAsync(IdentityResult.Success);

        // Act
        ActionResult<User> result = await _controller.PostUser(user);

        // Assert
        CreatedAtActionResult actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        User returnValue = Assert.IsType<User>(actionResult.Value);
        Assert.Equal(user, returnValue);
    }

    [Fact]
    public async Task DeleteUser_AsAnAdmin_ShouldRevokeToken()
    {
        // Arrange
        User user = new() { Id = "1", UserName = "userone", Fullname = "User One", Email = "userone@example.com" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Simulate an HTTP context with an admin user
        List<Claim> userClaims = new()
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Role, "Admin")
        };
        ClaimsIdentity identity = new(userClaims, "TestAuthType");
        ClaimsPrincipal claimsPrincipal = new(identity);

        Mock<IAuthenticationService> mockAuthService = new();
        mockAuthService
            .Setup(x => x.SignOutAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<AuthenticationProperties>()))
            .Returns(Task.CompletedTask);

        Mock<IServiceProvider> mockServiceProvider = new();
        mockServiceProvider
            .Setup(x => x.GetService(typeof(IAuthenticationService)))
            .Returns(mockAuthService.Object);

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

        _mockJwtRevocationService.Setup(j => j.RevokeUserTokensAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

        // Act
        IActionResult result = await _controller.DeleteUser("1");

        // Assert
        Assert.IsType<NoContentResult>(result);
        _mockJwtRevocationService.Verify(j => j.RevokeUserTokensAsync("1"), Times.Once);
        mockAuthService.Verify(x => x.SignOutAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<AuthenticationProperties>()), Times.Once);
    }

    [Fact]
    public async Task DeleteUser_NotAuthenticated_ShouldReturnForbidResult()
    {
        // Arrange
        User user = new() { Id = "1", UserName = "userone", Fullname = "User One", Email = "userone@example.com" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Simulate an HTTP context with no user
        DefaultHttpContext httpContext = new();
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        // Act
        IActionResult result = await _controller.DeleteUser("1");

        // Assert
        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async Task DeleteUser_NotAdmin_ShouldReturnForbidResult()
    {
        // Arrange
        User user = new() { Id = "1", UserName = "userone", Fullname = "User One", Email = "userone@example.com" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Simulate an HTTP context with a non-admin user
        List<Claim> userClaims = new()
        {
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, user.UserName),
        // No Admin role
    };
        ClaimsIdentity identity = new(userClaims, "TestAuthType");
        ClaimsPrincipal claimsPrincipal = new(identity);

        DefaultHttpContext httpContext = new()
        {
            User = claimsPrincipal
        };
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        // Act
        IActionResult result = await _controller.DeleteUser("1");

        // Assert
        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async Task DeleteUser_NonExistingId_ShouldReturnNotFound()
    {
        // Arrange
        User user = new() { Id = "1", UserName = "userone", Fullname = "User One", Email = "userone@example.com" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Simulate an HTTP context with an admin user
        List<Claim> userClaims = new()
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Role, "Admin")
        };
        ClaimsIdentity identity = new(userClaims, "TestAuthType");
        ClaimsPrincipal claimsPrincipal = new(identity);

        Mock<IAuthenticationService> mockAuthService = new();
        mockAuthService
            .Setup(x => x.SignOutAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<AuthenticationProperties>()))
            .Returns(Task.CompletedTask);

        Mock<IServiceProvider> mockServiceProvider = new();
        mockServiceProvider
            .Setup(x => x.GetService(typeof(IAuthenticationService)))
            .Returns(mockAuthService.Object);

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
        _mockUserManager.Setup(um => um.IsInRoleAsync(It.IsAny<User>(), "Admin")).ReturnsAsync(true);

        _mockJwtRevocationService.Setup(j => j.RevokeUserTokensAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

        // Act
        IActionResult result = await _controller.DeleteUser("99");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteUser_CurrentUser_ShouldRevokeToken()
    {
        // Arrange
        User currentUser = new() { Id = "1", UserName = "userone", Fullname = "User One", Email = "userone@example.com" };
        _context.Users.Add(currentUser);
        await _context.SaveChangesAsync();

        List<Claim> userClaims = new()
        {
            new Claim(ClaimTypes.NameIdentifier, currentUser.Id),
            new Claim(ClaimTypes.Name, currentUser.UserName)
        };
        ClaimsIdentity identity = new(userClaims, "TestAuthType");
        ClaimsPrincipal claimsPrincipal = new(identity);

        Mock<IAuthenticationService> mockAuthService = new();
        mockAuthService
            .Setup(x => x.SignOutAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<AuthenticationProperties>()))
            .Returns(Task.CompletedTask);

        Mock<IServiceProvider> mockServiceProvider = new();
        mockServiceProvider
            .Setup(x => x.GetService(typeof(IAuthenticationService)))
            .Returns(mockAuthService.Object);

        DefaultHttpContext httpContext = new()
        {
            User = claimsPrincipal,
            RequestServices = mockServiceProvider.Object
        };
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(currentUser);
        _mockUserManager.Setup(um => um.IsInRoleAsync(It.IsAny<User>(), "Admin")).ReturnsAsync(false);

        _mockJwtRevocationService.Setup(j => j.RevokeUserTokensAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

        // Act
        IActionResult result = await _controller.DeleteUser("1");

        // Assert
        Assert.IsType<NoContentResult>(result);
        _mockJwtRevocationService.Verify(j => j.RevokeUserTokensAsync("1"), Times.Once);
        mockAuthService.Verify(x => x.SignOutAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<AuthenticationProperties>()), Times.Once);
    }
}