﻿using Dot.Net.WebApi;
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

        public UserController(LocalDbContext context)
        {
            _context = context;
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

        // [Authorize(Policy = "User")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> PutUser(string id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest("The Id entered in the parameter is not the same as the Id enter in the body");
            }

            // Vérifiez que l'utilisateur connecté est l'utilisateur courant ou un administrateur
            User? currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser == null || (currentUser.Id != user.Id && !await _userManager.IsInRoleAsync(currentUser, "Admin")))
            // ajouter admin puisse aussi le faire
            {
                return Forbid();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound("User with this Id does not exist");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        [Route("add")]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            //  user.Id must be  set by the database automatically, so we set it to 0 to force it
            user.Id = 0;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
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