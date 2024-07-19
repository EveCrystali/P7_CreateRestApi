using Dot.Net.WebApi;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dot.Net.WebApi.Services;

namespace P7CreateRestApi.Controllers
{
    [LogApiCallAspect]
    [Route("ratings")]
    [ApiController]
    public class RatingController(LocalDbContext context, IUpdateService<Rating> updateService) : ControllerBase
    {
        private readonly LocalDbContext _context = context;
        private readonly IUpdateService<Rating> _updateService = updateService;

        [HttpGet]
        public async Task<ActionResult> GetRatings()
        {
            List<Rating> ratings = await _context.Ratings.ToListAsync();
            return ratings != null ? Ok(ratings) : BadRequest("Failed to get list of Ratings");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Rating>> GetRating(int id)
        {
            Rating? rating = await _context.Ratings.FindAsync(id);

            if (rating == null)
            {
                return NotFound("Rating with this Id does not exist");
            }

            return Ok(rating);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRating(int id, Rating rating)
        {
            return await _updateService.UpdateEntity(id, rating, RatingExists, t => t.Id);
        }

        [HttpPost]
        public async Task<ActionResult<Rating>> PostRating(Rating rating)
        {
            try
            {
                rating.Validate();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            // rating.Id must be set by the database automatically, so we set it to 0 to force it
            rating.Id = 0;

            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRating", new { id = rating.Id }, rating);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRating(int id)
        {
            Rating? rating = await _context.Ratings.FindAsync(id);
            if (rating == null)
            {
                return NotFound("Rating with this Id does not exist");
            }

            _context.Ratings.Remove(rating);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RatingExists(Rating rating)
        {
            return _context.Ratings.Any(e => e.Id == rating.Id);
        }
    }
}