using Dot.Net.WebApi;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Models;
using Dot.Net.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Controllers;

[LogApiCallAspect]
[Route("auth")]
[ApiController]
public class AuthentificationController(UserManager<User> userManager, IJwtService jwtService, LocalDbContext context) : ControllerBase
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IJwtService _jwtService = jwtService;
    private readonly LocalDbContext _context = context;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        User? user = await _userManager.FindByNameAsync(model.Username);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            user.LastLoginDate = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            IList<string> userRoles = await _userManager.GetRolesAsync(user);
            string token = _jwtService.GenerateToken(user.Id, user.UserName, userRoles.ToArray());
            RefreshToken refreshToken = _jwtService.GenerateRefreshToken(user.Id);

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return Ok(new { Token = token, RefreshToken = refreshToken.Token });
        }
        return Unauthorized();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        if (ModelState.IsValid)
        {
            User user = new() { UserName = model.Username, Fullname = model.Fullname, PasswordHash = model.Password };
            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                return Ok(new { Message = "User registered successfully" });
            }
            return BadRequest(result.Errors);
        }
        return BadRequest(ModelState);
    }

    [Authorize(Policy = "RequireUserRole")]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] Dot.Net.WebApi.Models.RefreshRequest model)
    {
        RefreshToken? refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == model.RefreshToken);
        if (refreshToken == null || refreshToken.ExpiryDate < DateTime.UtcNow || refreshToken.IsRevoked)
        {
            return Unauthorized();
        }

        User? user = await _userManager.FindByIdAsync(refreshToken.UserId);
        if (user == null || !user.IsUserActive())
        {
            return Unauthorized();
        }

        IList<string> userRoles = await _userManager.GetRolesAsync(user);
        string newToken = _jwtService.GenerateToken(user.Id, user.UserName, userRoles.ToArray());
        RefreshToken newRefreshToken = _jwtService.GenerateRefreshToken(user.Id);

        // Revok previous refresh token
        refreshToken.IsRevoked = true;
        _context.RefreshTokens.Update(refreshToken);

        // Add new refresh token
        _context.RefreshTokens.Add(newRefreshToken);

        // Delete old refresh tokens
        List<RefreshToken> oldTokens = await _context.RefreshTokens
            .Where(rt => rt.UserId == user.Id && (rt.ExpiryDate < DateTime.UtcNow || rt.IsRevoked))
            .ToListAsync();
        _context.RefreshTokens.RemoveRange(oldTokens);

        await _context.SaveChangesAsync();

        return Ok(new { Token = newToken, RefreshToken = newRefreshToken.Token });
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpPost("revoke")]
    public async Task<IActionResult> RevokeTokens([FromBody] RevokeTokensRequest model)
    {
        User? user = await _userManager.FindByIdAsync(model.UserId);
        if (user == null)
        {
            return NotFound();
        }

        List<RefreshToken> userTokens = [.. _context.RefreshTokens.Where(rt => rt.UserId == model.UserId)];
        foreach (RefreshToken? token in userTokens)
        {
            token.IsRevoked = true;
        }

        _context.RefreshTokens.UpdateRange(userTokens);
        await _context.SaveChangesAsync();

        return Ok();
    }
}