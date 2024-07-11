using System.ComponentModel.DataAnnotations;
using Dot.Net.WebApi;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace P7CreateRestApi.Controllers
{
    [LogApiCallAspect]
    [Route("[controller]")]
    [ApiController]
    public class BidListController : ControllerBase
    {
        private readonly LocalDbContext _context;

        public BidListController(LocalDbContext context)
        {
            _context = context;
        }

        
        [HttpGet("list")]
        public async Task<ActionResult> GetBidLists()
        {
            List<BidList> BidLists = await _context.BidLists.ToListAsync();
            return BidLists != null ? Ok(BidLists) : BadRequest("Failed to get list of BidLists");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BidList>> GetBidList(int id)
        {
            BidList? bidList = await _context.BidLists.FindAsync(id);

            if (bidList == null)
            {
                return NotFound("BidList with this Id doesn't exist");
            }

            return bidList;
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> PutBidList(int id, BidList bidList)
        {
            if (id != bidList.BidListId)
            {
                return BadRequest("The Id entered in the parameter is not the same as the Id enter in the body");
            }

            try
            {
                bidList.Validate();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            _context.Entry(bidList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BidListExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost("add")]
        public async Task<ActionResult<BidList>> PostBidList(BidList bidList)
        {

            try
            {
                bidList.Validate();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            // bidList.BidListId must be  set by the database automatically, so we set it to 0 to force it
            bidList.BidListId = 0;

            _context.BidLists.Add(bidList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBidList", new { id = bidList.BidListId }, bidList);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteBidList(int id)
        {
            BidList? bidList = await _context.BidLists.FindAsync(id);
            if (bidList == null)
            {
                return NotFound("BidList with this Id doesn't exist");
            }

            _context.BidLists.Remove(bidList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BidListExists(int id)
        {
            return _context.BidLists.Any(e => e.BidListId == id);
        }
    }
}