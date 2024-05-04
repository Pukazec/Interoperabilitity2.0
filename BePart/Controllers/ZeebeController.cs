using BePart.Data_Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BePart.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ZeebeController(IZeebeService zeebeService) : ControllerBase
    {
        private readonly IZeebeService _zeebeService = zeebeService;

        [HttpGet("/status")]
        public async Task<string> Get()
        {
            return (await _zeebeService.Status()).ToString() ?? "";
        }

        [HttpGet("/start")]
        public async Task<string> StartWorkflowInstance()
        {
            var instance = await _zeebeService.StartWorkflowInstance("test-process");
            return instance;
        }
    }
}
