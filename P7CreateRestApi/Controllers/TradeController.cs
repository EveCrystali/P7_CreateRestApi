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
    public class TradeController : ControllerBase
    {
        private readonly LocalDbContext _context;

        public TradeController(LocalDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("list")]
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

            return trade;
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> PutTrade(int id, Trade trade)
        {
            if (id != trade.TradeId)
            {
                return BadRequest("The Id entered in the parameter is not the same as the Id enter in the body");
            }

            try
            {
                trade.Validate();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            _context.Entry(trade).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TradeExists(id))
                {
                    NotFound("Trade with this Id does not exist");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost("add")]
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

        [HttpDelete("delete/{id}")]
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

        private bool TradeExists(int id)
        {
            return _context.Trades.Any(e => e.TradeId == id);
        }
    }
}