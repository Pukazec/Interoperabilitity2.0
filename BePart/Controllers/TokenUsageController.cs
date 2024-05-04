using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BePart.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class TokenUsageController : Controller
    {
        [HttpGet]
        public async Task<string> Get()
        {
            return "Bokic popi si sokic.";
        }
    }
}
