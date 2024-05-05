using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BePart.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TokenUsageController() : Controller
    {
        [HttpGet("user")]
        [Authorize(Roles = "User")]
        public async Task<string> Get()
        {
            return "Bokic popi si sokic.";
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<string> GetAdmin()
        {
            return "Bokic popi si sampanjcic.";
        }
    }
}
