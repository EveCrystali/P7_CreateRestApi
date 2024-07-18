using Dot.Net.WebApi;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace P7CreateRestApi.Controllers
{
    [LogApiCallAspect]
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly LocalDbContext _context;

        private readonly UserManager<User> _userManager;

        private readonly IJwtRevocationService _jwtRevocationService;

        public UserController(LocalDbContext context, UserManager<User> userManager, IJwtRevocationService jwtRevocationService)
        {
            _context = context;
            _userManager = userManager;
            _jwtRevocationService = jwtRevocationService;
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> GetUsers()
        {
            List<User> users = await _context.Users.ToListAsync();
            return users != null ? Ok(users) : BadRequest("Failed to get list of Users");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            User? user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound("No User found with this Id");
            }

            return Ok(user);
        }

        [Authorize(Policy = "User")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> PutUser(string id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest("The Id entered in the parameter is not the same as the Id enter in the body");
            }

            // Verify that the connected user is the user being updated or an administrator
            User? currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser == null || (currentUser.Id != user.Id && !await _userManager.IsInRoleAsync(currentUser, "Admin")))
            {
                return Forbid();
            }

            try
            {
                user.Validate();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            User? existingUser = await _userManager.FindByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound("User with this Id does not exist.");
            }

            // Update only allowed properties to prevent overposting
            existingUser.Fullname = user.Fullname;
            existingUser.Email = user.Email;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.LastLoginDate = user.LastLoginDate;

            // Use UserManager to update user information
            IdentityResult result = await _userManager.UpdateAsync(existingUser);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }

        [HttpPost]
        [Route("add")]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            try
            {
                user.Validate();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            user.Id = null;

            IdentityResult result = await _userManager.CreateAsync(user, user.PasswordHash);

            if (result.Succeeded)
            {
                return CreatedAtAction("GetUser", new { id = user.Id }, user);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            // Verify that the connected user is the user being deleted or an administrator
            User? currentUser = await _userManager.GetUserAsync(HttpContext.User);

            if (currentUser != null && (await _userManager.IsInRoleAsync(currentUser, "Admin") || id == currentUser.Id))
            {
                User? user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                if (id == currentUser.Id)
                {
                    // Revoke JWT token and log out the user
                    await _jwtRevocationService.RevokeUserTokensAsync(currentUser.Id);

                    if (HttpContext != null && HttpContext.RequestServices != null)
                    {
                        IAuthenticationService? authService = HttpContext.RequestServices.GetService<IAuthenticationService>();
                        if (authService != null)
                        {
                            try
                            {
                                await authService.SignOutAsync(HttpContext, IdentityConstants.ApplicationScheme, null);
                            }
                            catch (InvalidOperationException)
                            {
                                // Ignore the exception in test environment
                            }
                        }
                    }
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            else
            {
                return Forbid();
            }
        }
    }
}