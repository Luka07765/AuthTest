using AuthLearning.Models;
using AuthLearning.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (succeeded, errors) = await _authService.RegisterAsync(model);
            if (!succeeded)
            {
                foreach (var error in errors)
                    ModelState.AddModelError(string.Empty, error);

                return BadRequest(ModelState);
            }

            return Ok(new { Message = "User registered successfully!" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (succeeded, message, tokens) = await _authService.LoginAsync(model);
            if (!succeeded)
                return Unauthorized(new { Message = message });

            return Ok(new
            {
                Message = message,
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken,
                ExpiresIn = tokens.ExpiresIn
            });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModel model)
        {
            var newTokens = await _authService.RefreshTokenAsync(model.AccessToken, model.RefreshToken);
            if (newTokens == null)
                return Unauthorized(new { Message = "Invalid token or refresh token" });

            return Ok(new
            {
                AccessToken = newTokens.AccessToken,
                RefreshToken = newTokens.RefreshToken,
                ExpiresIn = newTokens.ExpiresIn
            });
        }
    }
}
