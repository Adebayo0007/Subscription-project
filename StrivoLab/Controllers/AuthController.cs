using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StrivoLab.DTOs;
using StrivoLab.Service.Interfaces;

namespace StrivoLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public AuthController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var result = await _tokenService.LoginAsync(req.service_id, req.password);
            if (result == null) return Unauthorized(new { message = "Invalid service credentials" });
            return Ok(result);
        }
    }
}
