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
    [Route("users")]
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
        public async Task<IActionResult> GetUsers()
        {
            List<User> users = await _context.Users.ToListAsync();
            return users != null ? Ok(users) : BadRequest("Failed to get list of Users");
        }

        [Authorize(Policy = "RequireUserRole")]
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

        [Authorize(Policy = "RequireUserRole")]
        [LogApiCallAspect]
        [HttpPut("{id}")]
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

            return Ok(existingUser);
        }


        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            User? currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser == null)
            {
                return Forbid();
            }

            bool isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");
            if (!isAdmin && id != currentUser.Id)
            {
                return Forbid();
            }

            User? userToDelete = await _context.Users.FindAsync(id);
            if (userToDelete == null)
            {
                return NotFound();
            }

            await _jwtRevocationService.RevokeUserTokensAsync(currentUser.Id);

            if (id == currentUser.Id)
            {
                await SignOutCurrentUserAsync();
            }

            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task SignOutCurrentUserAsync()
        {
            if (HttpContext?.RequestServices == null)
            {
                return;
            }

            IAuthenticationService? authService = HttpContext.RequestServices.GetService<IAuthenticationService>();
            if (authService == null)
            {
                return;
            }

            try
            {
                await authService.SignOutAsync(HttpContext, IdentityConstants.ApplicationScheme, null);
            }
            catch (InvalidOperationException)
            {
                // Ignore exception in test environnement
            }
        }
    }
}