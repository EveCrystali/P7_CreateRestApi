using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Controllers
{
    [LogApiCallAspect]
    [ApiController]
    [Route("[controller]")]
    public class CurvePointController(ICurvePointService curvePointService, LocalDbContext context) : ControllerBase
    {
        private readonly ICurvePointService _curvePointService = curvePointService;
        private readonly LocalDbContext _context = context;

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddCurvePoint([FromBody] CurvePoint curvePoint)
        {
            if (!TryValidateModel(curvePoint)) return BadRequest(ModelState);

            if (!_curvePointService.ValidateCurvePoint(curvePoint)) return BadRequest("Data not valid");

            if (_curvePointService.CurvePointExistsByCurveId(curvePoint.CurveId)) return BadRequest("CurvePoint with this Id already exists");

            await _curvePointService.SaveCurvePoint(curvePoint);

            return Ok();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCurvePoint(int id)
        {
            CurvePoint? curvePoint = await _curvePointService.GetCurvePointById(id);
            if (curvePoint == null)
            {
                return NotFound("CurvePoint with this Id doesn't exist");
            }
            return Ok(curvePoint);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteCurvePoint(int id)
        {
            CurvePoint? curvePoint = await _curvePointService.GetCurvePointById(id);
            if (curvePoint == null) return NotFound("CurvePoint with this Id doesnt exist");
            int returnDeletion = await _curvePointService.DeleteCurvePoint(curvePoint);

            return returnDeletion == 0 ? Ok() : BadRequest("Failed to delete CurvePoint");
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> GetCurvePoints()
        {
            List<CurvePoint> curvePoints = await _context.CurvePoints.ToListAsync();
            return curvePoints != null ? Ok(curvePoints) : BadRequest("Failed to get list of CurvePoints");
        }

        [HttpPost]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateCurvePoint(int id, [FromBody] CurvePoint curvePoint)
        {
            if (!_curvePointService.ValidateCurvePoint(curvePoint)) return BadRequest("Data not valid");
            if (!_curvePointService.CurvePointExistsById(id)) return NotFound("CurvePoint not found");

            CurvePoint? existingCurvePoint = await _curvePointService.GetCurvePointById(id);
            existingCurvePoint.Term = curvePoint.Term;
            existingCurvePoint.CurvePointValue = curvePoint.CurvePointValue;

            await _curvePointService.UpdateCurvePoint(existingCurvePoint);

            return Ok(existingCurvePoint);
        }
    }
}