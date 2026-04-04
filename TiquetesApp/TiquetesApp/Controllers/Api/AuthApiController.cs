using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TiquetesApp.Models.ViewModels;
using TiquetesApp.Services;

namespace TiquetesApp.Controllers.Api
{
    [Route("api/auth")]
    [ApiController]
    public class AuthApiController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthApiController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                return BadRequest(new { message = "Datos inválidos.", errors });
            }

            var (succeeded, registerErrors) = await _authService.RegisterAsync(model);

            if (!succeeded)
                return BadRequest(new { message = "Error al registrar.", errors = registerErrors });

            return Ok(new { message = "Registro exitoso. Por favor inicia sesión." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Datos inválidos." });

            var (succeeded, token, errorMessage) = await _authService.LoginAsync(model);

            if (!succeeded)
                return Unauthorized(new { message = errorMessage });

            return Ok(new { message = "Login exitoso.", token });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return Ok(new { message = "Sesión cerrada correctamente." });
        }
    }
}