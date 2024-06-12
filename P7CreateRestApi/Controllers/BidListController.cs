using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;

namespace P7CreateRestApi.Controllers
{
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
        public async Task<ActionResult<IEnumerable<BidList>>> GetBidLists()
        {
            return await _context.BidLists.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BidList>> GetBidList(int id)
        {
            var bidList = await _context.BidLists.FindAsync(id);

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

        [HttpPost("add/{id}")]
        public async Task<ActionResult<BidList>> PostBidList(BidList bidList)
        {
            _context.BidLists.Add(bidList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBidList", new { id = bidList.BidListId }, bidList);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteBidList(int id)
        {
            var bidList = await _context.BidLists.FindAsync(id);
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