using Dot.Net.WebApi;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dot.Net.WebApi.Services;

namespace P7CreateRestApi.Controllers
{
    [LogApiCallAspect]
    [Route("rulenames")]
    [ApiController]
    public class RuleNameController(LocalDbContext context, IUpdateService<RuleName> updateService) : ControllerBase
    {
        private readonly LocalDbContext _context = context;
        private readonly IUpdateService<RuleName> _updateService = updateService;

        [HttpGet]
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

            return Ok(ruleName);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRuleName(int id, RuleName ruleName)
        {
            return await _updateService.UpdateEntity(id, ruleName, RuleNameExists, t => t.Id);
        }

        [HttpPost]
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

        [HttpDelete("{id}")]
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

        private bool RuleNameExists(RuleName ruleName)
        {
            return _context.RuleNames.Any(e => e.Id == ruleName.Id);
        }
    }
}