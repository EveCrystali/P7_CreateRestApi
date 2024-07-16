﻿using Dot.Net.WebApi;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dot.Net.WebApi.Services;

namespace P7CreateRestApi.Controllers
{
    [LogApiCallAspect]
    [Route("[controller]")]
    [ApiController]
    public class BidListController(LocalDbContext context, UpdateService updateService) : ControllerBase
    {
        private readonly LocalDbContext _context = context;
        private readonly UpdateService _updateService = updateService;

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
            return await _updateService.UpdateEntity(id, bidList, BidListExists, t => t.BidListId);
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

            // bidList.BidListId must be set by the database automatically, so we set it to 0 to force it
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

        private bool BidListExists(BidList bidList)
        {
            return _context.BidLists.Any(e => e.BidListId == bidList.BidListId);
        }
    }
}