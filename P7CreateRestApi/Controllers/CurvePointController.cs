using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Controllers
{
    [LogApiCallAspect]
    [Route("curvepoints")]
    [ApiController]
    public class CurvePointController(LocalDbContext context, IUpdateService<CurvePoint> updateService) : ControllerBase
    {
        private readonly LocalDbContext _context = context;
        private readonly IUpdateService<CurvePoint> _updateService = updateService;

        [HttpGet]
        public async Task<IActionResult> GetCurvePoints()
        {
            List<CurvePoint> curvePoints = await _context.CurvePoints.ToListAsync();
            return curvePoints != null ? Ok(curvePoints) : BadRequest("Failed to get list of CurvePoints");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCurvePoint(int id)
        {
            CurvePoint? curvePoint = await _context.CurvePoints.FindAsync(id);

            if (curvePoint == null)
            {
                return NotFound("CurvePoint with this Id does not exist");
            }

            return Ok(curvePoint);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurvePoint(int id, CurvePoint curvePoint)
        {
            return await _updateService.UpdateEntity(id, curvePoint, CurvePointExists, t => t.Id);
        }

        [HttpPost]
        public async Task<IActionResult> PostCurvePoint(CurvePoint curvePoint)
        {
            try
            {
                curvePoint.Validate();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            // curvePoint.Id must be set by the database automatically, so we set it to 0 to force it
            curvePoint.Id = 0;

            _context.CurvePoints.Add(curvePoint);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCurvePoint", new { id = curvePoint.Id }, curvePoint);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurvePoint(int id)
        {
            CurvePoint? curvePoint = await _context.CurvePoints.FindAsync(id);
            if (curvePoint == null)
            {
                return NotFound("CurvePoint with this Id does not exist");
            }
            _context.CurvePoints.Remove(curvePoint);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CurvePointExists(CurvePoint curvePoint)
        {
            return _context.CurvePoints.Any(e => e.Id == curvePoint.Id);
        }
    }
}