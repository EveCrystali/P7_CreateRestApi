using System.Collections.Generic;
using System.Threading.Tasks;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using P7CreateRestApi.Controllers;
using Xunit;
using P7CreateRestApi.Tests;
using System.Security.Claims;
using Dot.Net.WebApi.Services;


namespace P7CreateRestApi.Tests;

public class UserControllerTests : TestBase<User>
{
    private readonly UserController _controller;
    private readonly Mock<UserManager<User>> _mockUserManager;
    private readonly Mock<JwtRevocationService> _mockJwtRevocationService;

    public UserControllerTests()
    {
        _mockUserManager = MockUserManager<User>();
        _mockJwtRevocationService = new Mock<JwtRevocationService>();
        _controller = new UserController(_context, _mockUserManager.Object, _mockJwtRevocationService.Object);
    }

    private Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
    {
        var store = new Mock<IUserStore<TUser>>();
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
        var result = await _controller.GetUsers();

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<User>>(actionResult.Value);
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
        var result = await _controller.GetUser("1");

        // Assert
        var actionResult = Assert.IsType<ActionResult<User>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var returnValue = Assert.IsType<User>(okResult.Value);
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
        var userClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, "Admin")
         };
        var identity = new ClaimsIdentity(userClaims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        // Act
        var result = await _controller.PutUser("1", updatedUser);

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
        var result = await _controller.PostUser(user);

        // Assert
        var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnValue = Assert.IsType<User>(actionResult.Value);
        Assert.Equal(user, returnValue);
    }

    [Fact]
    public async Task DeleteUser_ExistingIdAndAdmin_ShouldReturnNoContent()
    {
        // Arrange
        User user = new() { Id = "1", UserName = "userone", Fullname = "User One", Email = "userone@example.com" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Simulate an HTTP context with an admin user
        var userClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, "Admin")
        };
        var identity = new ClaimsIdentity(userClaims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        // Act
        var result = await _controller.DeleteUser("1");

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteUser_NotAuthenticated_ShouldReturnUnauthorized()
    {
        // Arrange
        User user = new() { Id = "1", UserName = "userone", Fullname = "User One", Email = "userone@example.com" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Simulate an HTTP context with no user
        var httpContext = new DefaultHttpContext();
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        // Act
        var result = await _controller.DeleteUser("1");

        // Assert
        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async Task DeleteUser_NotAdmin_ShouldReturnForbid()
    {
        // Arrange
        User user = new() { Id = "1", UserName = "userone", Fullname = "User One", Email = "userone@example.com" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Simulate an HTTP context with a non-admin user
        var userClaims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, user.UserName),
        // No Admin role
    };
        var identity = new ClaimsIdentity(userClaims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        // Act
        var result = await _controller.DeleteUser("1");

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
        var userClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, "Admin")
        };
        var identity = new ClaimsIdentity(userClaims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        // Act
        var result = await _controller.DeleteUser("99");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
