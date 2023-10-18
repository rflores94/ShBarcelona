using Microsoft.AspNetCore.Mvc;
using ShBarcelona.Services.Area;

namespace ShBarcelona.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AreaController : Controller
    {
        private readonly IAreaService _areaService;

        public AreaController(
            IAreaService areaService
            )
        {
            _areaService = areaService;
        }

        [HttpGet("area/{areaId}")]
        public async Task<IActionResult> GetArea(int areaId)
        {
            try
            {
                return Ok(await _areaService.GetByAreaIdAsync(areaId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("areas")]
        public async Task<IActionResult> GetAreas()
        {
            try
            {
                return Ok(await _areaService.GetAllAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddArea([FromBody] AreaDto request)
        {
            try
            {
                return Created("area", await _areaService.AddAsync(request));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
