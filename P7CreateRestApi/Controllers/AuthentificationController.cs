using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using P7CreateRestApi.Models;
using Dot.Net.WebApi;
using Dot.Net.WebApi.Services;

namespace P7CreateRestApi.Controllers
{
    [LogApiCallAspect]
    [Route("[controller]")]
    [ApiController]
    public class AuthentificationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtService _jwtService;

        public AuthentificationController(UserManager<User> userManager, JwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            User? user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var token = _jwtService.GenerateToken(user.Id, user.UserName, userRoles.ToArray());
                return Ok(new { Token = token });
            }
            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new() { UserName = model.Username, Fullname = model.Fullname, PasswordHash = model.Password };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    return Ok(new { Message = "User registered successfully" });
                }
                return BadRequest(result.Errors);
            }
            return BadRequest(ModelState);
        }
    }
}