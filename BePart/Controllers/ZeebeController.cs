using BePart.Data_Service;
using Microsoft.AspNetCore.Mvc;

namespace BePart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZeebeController : ControllerBase
    {
        private readonly IZeebeService _zeebeService;

        public ZeebeController(IZeebeService zeebeService)
        {
            _zeebeService = zeebeService;
        }

        [Route("/status")]
        [HttpGet]
        public async Task<string> Get()
        {
            return (await _zeebeService.Status()).ToString();
        }

        [Route("/start")]
        [HttpGet]
        public async Task<string> StartWorkflowInstance()
        {
            var instance = await _zeebeService.StartWorkflowInstance("test-process");
            return instance;
        }
    }
}
