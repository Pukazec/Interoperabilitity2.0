using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedData;

namespace BePart.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authService)
        {
            _authenticationService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser(LoginUser user)
        {
            if (await _authenticationService.RegisterUser(user))
            {
                return Ok("Successfully done");
            }
            return BadRequest("Something went wrong");
        }

        [HttpPost("RegisterRole")]
        public async Task<IActionResult> RegisterRole(string role)
        {
            if (await _authenticationService.RegisterRole(role))
            {
                return Ok("Successfully done");
            }
            return BadRequest("Something went wrong");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var loginResult = await _authenticationService.Login(user);
            if (loginResult.IsLogedIn)
            {
                return Ok(loginResult);
            }
            return Unauthorized();
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenModel model)
        {
            var loginResult = await _authenticationService.RefreshToken(model);
            if (loginResult.IsLogedIn)
            {
                return Ok(loginResult);
            }
            return Unauthorized();
        }
    }
}
