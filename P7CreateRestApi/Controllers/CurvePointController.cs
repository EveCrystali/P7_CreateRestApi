using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurvePointController : ControllerBase
    {
        private readonly ICurvePointService _curvePointService;

        public CurvePointController(ICurvePointService curvePointService)
        {
            _curvePointService = curvePointService;
        }

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

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteCurvePoint(int id)
        {
            CurvePoint? curvePoint = await _curvePointService.GetCurvePointById(id);
            if (curvePoint == null) return BadRequest("CurvePoint with this Id doesnt exist");
            int returnDeletion = await _curvePointService.DeleteCurvePoint(curvePoint);

            return returnDeletion == 0 ? Ok() : BadRequest("Failed to delete CurvePoint");
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> Home()
        {
            return Ok(await _curvePointService.GetAllCurvePoints());
        }

        [HttpPost]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateCurvePoint(int id, [FromBody] CurvePoint curvePoint)
        {
            if (!_curvePointService.ValidateCurvePoint(curvePoint)) return BadRequest("Data not valid");
            if (!_curvePointService.CurvePointExistsById(id)) return BadRequest("CurvePoint not found");

            CurvePoint? existingCurvePoint = await _curvePointService.GetCurvePointById(id);
            existingCurvePoint.Term = curvePoint.Term;
            existingCurvePoint.CurvePointValue = curvePoint.CurvePointValue;

            await _curvePointService.UpdateCurvePoint(existingCurvePoint);

            return Ok(existingCurvePoint);
        }
    }
}