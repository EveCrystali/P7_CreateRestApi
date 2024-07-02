using Dot.Net.WebApi;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace P7CreateRestApi.Controllers
{
    [LogApiCallAspect]
    [Route("[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly LocalDbContext _context;

        public RatingController(LocalDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("list")]
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

            return rating;
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> PutRating(int id, Rating rating)
        {
            if (id != rating.Id)
            {
                return BadRequest("The Id entered in the parameter is not the same as the Id enter in the body");
            }

            _context.Entry(rating).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RatingExists(id))
                {
                    NotFound("Rating with this Id does not exist");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost("add")]
        public async Task<ActionResult<Rating>> PostRating(Rating rating)
        {
            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRating", new { id = rating.Id }, rating);
        }

        [HttpDelete("delete/{id}")]
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

        private bool RatingExists(int id)
        {
            return _context.Ratings.Any(e => e.Id == id);
        }
    }
}