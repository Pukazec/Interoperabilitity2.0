using BePart.Data;
using Microsoft.AspNetCore.Mvc;
using SharedData;

namespace BePart.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HardwareController : ControllerBase
    {
        private readonly IHardwareService _hardwareService;

        public HardwareController(IHardwareService hardwareService)
        {
            _hardwareService = hardwareService;
        }

        [HttpGet]
        public IActionResult GetAllHardware()
        {
            var hardware = _hardwareService.GetAllHardware();
            return Ok(hardware);
        }

        [HttpGet("{id}")]
        public IActionResult GetHardwareById(int id)
        {
            var hardware = _hardwareService.GetHardwareById(id);
            if (hardware == null)
            {
                return NotFound();
            }
            return Ok(hardware);
        }

        [HttpPost]
        public IActionResult AddHardware([FromBody] Hardware hardware)
        {
            _hardwareService.AddHardware(hardware);
            return CreatedAtAction(nameof(GetHardwareById), new { id = hardware.Id }, hardware);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateHardware(int id, [FromBody] Hardware hardware)
        {
            if (id != hardware.Id)
            {
                return BadRequest();
            }

            var existingHardware = _hardwareService.GetHardwareById(id);
            if (existingHardware == null)
            {
                return NotFound();
            }

            _hardwareService.UpdateHardware(hardware);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteHardware(int id)
        {
            var hardware = _hardwareService.GetHardwareById(id);
            if (hardware == null)
            {
                return NotFound();
            }

            _hardwareService.DeleteHardware(id);
            return NoContent();
        }
    }
}
