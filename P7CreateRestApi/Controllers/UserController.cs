using Dot.Net.WebApi;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Models;
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

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, UserUpdateModel userModel)
        {
            if (id != userModel.Id)
            {
                return BadRequest("The Id entered in the parameter is not the same as the Id enter in the body");
            }

            // Verify that the connected user is the user being updated or an administrator
            User? currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser == null || (currentUser.Id != userModel.Id && !await _userManager.IsInRoleAsync(currentUser, "Admin")))
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User? existingUser = await _userManager.FindByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound("User with this Id does not exist.");
            }

            // Update only allowed properties to prevent overposting
            existingUser.Fullname = userModel.Fullname;
            existingUser.Email = userModel.Email;
            existingUser.PhoneNumber = userModel.PhoneNumber;

            // Update the role if provided and the current user is an admin
            if ((userModel.Role == "User" || userModel.Role == "Trader") && await _userManager.IsInRoleAsync(currentUser, "Admin"))
            {
                // Get current roles
                IList<string> currentRoles = await _userManager.GetRolesAsync(existingUser);

                // Check if the new role is different from the current role
                if (!currentRoles.Contains(userModel.Role))
                {
                    // Remove all current roles
                    IdentityResult removeResult = await _userManager.RemoveFromRolesAsync(existingUser, currentRoles);
                    if (!removeResult.Succeeded)
                    {
                        return BadRequest(removeResult.Errors);
                    }

                    // Add the new role
                    IdentityResult addResult = await _userManager.AddToRoleAsync(existingUser, userModel.Role);
                    if (!addResult.Succeeded)
                    {
                        return BadRequest(addResult.Errors);
                    }
                }
            }

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