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
    public class RuleNameController : ControllerBase
    {
        private readonly LocalDbContext _context;

        public RuleNameController(LocalDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("list")]
        public async Task<ActionResult> GetRuleNames()
        {
            List<RuleName> ruleNames = await _context.RuleNames.ToListAsync();
            return ruleNames != null ? Ok(ruleNames) : BadRequest("Failed to get list of RuleNames");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RuleName>> GetRuleName(int id)
        {
            RuleName? ruleName = await _context.RuleNames.FindAsync(id);

            if (ruleName == null)
            {
                return NotFound("RuleName with this Id does not exist");
            }

            return ruleName;
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> PutRuleName(int id, RuleName ruleName)
        {
            if (id != ruleName.Id)
            {
                return BadRequest("The Id entered in the parameter is not the same as the Id enter in the body");
            }

            try
            {
                ruleName.Validate();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            _context.Entry(ruleName).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RuleNameExists(id))
                {
                    return NotFound("RuleName with this Id does not exist");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost("add")]
        public async Task<ActionResult<RuleName>> PostRuleName(RuleName ruleName)
        {
            try
            {
                ruleName.Validate();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            // ruleName.Id must be set by the database automatically, so we set it to 0 to force it
            ruleName.Id = 0;

            _context.RuleNames.Add(ruleName);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRuleName", new { id = ruleName.Id }, ruleName);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteRuleName(int id)
        {
            RuleName? ruleName = await _context.RuleNames.FindAsync(id);
            if (ruleName == null)
            {
                return NotFound("RuleName with this Id does not exist");
            }

            _context.RuleNames.Remove(ruleName);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RuleNameExists(int id)
        {
            return _context.RuleNames.Any(e => e.Id == id);
        }
    }
}