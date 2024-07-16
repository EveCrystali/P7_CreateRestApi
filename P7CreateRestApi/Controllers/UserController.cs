using Dot.Net.WebApi;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
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

        public UserController(LocalDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> GetUsers()
        {
            List<User> users = await _context.Users.ToListAsync();
            return users != null ? Ok(users) : BadRequest("Failed to get list of Users");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            User? user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound("No User found with this Id");
            }

            return user;
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

            IdentityResult result = await _userManager.CreateAsync(user, user.PasswordHash); // Assure-toi que PasswordHash est correctement géré

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
        public async Task<IActionResult> DeleteUser(int id)
        {
            User? user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}