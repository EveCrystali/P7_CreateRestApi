﻿using Dot.Net.WebApi;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace P7CreateRestApi.Controllers
{
    [LogApiCallAspect]
    [Route("trades")]
    [ApiController]
    public class TradeController(LocalDbContext context, IUpdateService<Trade> updateService) : ControllerBase
    {
        private readonly LocalDbContext _context = context;
        private readonly IUpdateService<Trade> _updateService = updateService;

        [HttpGet]
        public async Task<ActionResult> GetTrades()
        {
            List<Trade> trades = await _context.Trades.ToListAsync();
            return trades != null ? Ok(trades) : BadRequest("Failed to get list of Trades");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Trade>> GetTrade(int id)
        {
            Trade? trade = await _context.Trades.FindAsync(id);

            if (trade == null)
            {
                return NotFound("Trade with this Id does not exist");
            }

            return Ok(trade);
        }

        [Authorize(Policy = "RequireTraderRole")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrade(int id, Trade trade)
        {
            return await _updateService.UpdateEntity(id, trade, TradeExists, t => t.TradeId);
        }

        [Authorize(Policy = "RequireTraderRole")]
        [HttpPost]
        public async Task<ActionResult<Trade>> PostTrade(Trade trade)
        {
            try
            {
                trade.Validate();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            // trade.TradeId must be set by the database automatically, so we set it to 0 to force it
            trade.TradeId = 0;

            _context.Trades.Add(trade);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTrade", new { id = trade.TradeId }, trade);
        }

        [Authorize(Policy = "RequireTraderRole")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrade(int id)
        {
            Trade? trade = await _context.Trades.FindAsync(id);
            if (trade == null)
            {
                return NotFound("Trade with this Id does not exist");
            }

            _context.Trades.Remove(trade);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TradeExists(Trade trade)
        {
            return _context.Trades.Any(e => e.TradeId == trade.TradeId);
        }
    }
}