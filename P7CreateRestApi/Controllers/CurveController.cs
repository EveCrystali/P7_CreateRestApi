using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurveController : ControllerBase
    {
        private readonly ICurvePointService _curvePointService;

        public CurveController(ICurvePointService curvePointService)
        {
            _curvePointService = curvePointService;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddCurvePoint([FromBody] CurvePoint curvePoint)
        {
            if (!TryValidateModel(curvePoint)) return BadRequest(ModelState);

            if (!_curvePointService.ValidateCurvePoint(curvePoint)) return BadRequest("Data not valid");

            await _curvePointService.SaveCurvePoint(curvePoint);

            return Ok();
        }

        [HttpPost]
        [Route("validate")]
        public async Task<IActionResult> ValidateAsync([FromBody] CurvePoint curvePoint)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            else
            {
                if (!_curvePointService.ValidateCurvePoint(curvePoint)) return BadRequest("Data not valid");
                else
                {
                    await _curvePointService.SaveCurvePoint(curvePoint);
                    var allCurvePoints = await _curvePointService.GetAllCurvePoints();
                    return Ok(allCurvePoints);
                }
            }

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteBid(int id)
        {
            // TODO: Find Curve by Id and delete the Curve, return to Curve list
            return Ok();
        }

        [HttpGet]
        [Route("list")]
        public IActionResult Home()
        {
            return Ok();
        }

        [HttpGet]
        [Route("update/{id}")]
        public IActionResult ShowUpdateForm(int id)
        {
            // TODO: get CurvePoint by Id and to model then show to the form
            return Ok();
        }

        [HttpPost]
        [Route("update/{id}")]
        public IActionResult UpdateCurvePoint(int id, [FromBody] CurvePoint curvePoint)
        {
            // TODO: check required fields, if valid call service to update Curve and return Curve list
            return Ok();
        }
    }
}