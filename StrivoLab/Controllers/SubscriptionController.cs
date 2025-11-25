using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StrivoLab.DTOs;
using StrivoLab.Service.Interfaces;

namespace StrivoLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ITokenService tokenService, ISubscriptionService subscriptionService)
        {
            _tokenService = tokenService;
            _subscriptionService = subscriptionService;
        }

        private async Task<(bool ok, string reason)> EnsureValidToken(string serviceId, string tokenId)
        {
            return await _tokenService.ValidateTokenAsync(serviceId, tokenId);
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe([FromBody] SubscribeRequest req)
        {
            var (ok, reason) = await EnsureValidToken(req.service_id, req.token_id);
            if (!ok)
            {
                if (reason == "Token ID expired") return Unauthorized(new { message = reason });
                return BadRequest(new { message = reason });
            }

            var (created, message, subscriptionId) = await _subscriptionService.SubscribeAsync(req.service_id, req.phone_number);
            if (!created && message == "User is already subscribed") return Conflict(new { message });
            return Ok(new SubscribeResponse(subscriptionId ?? string.Empty, message));
        }

        [HttpPost("unsubscribe")]
        public async Task<IActionResult> Unsubscribe([FromBody] SubscribeRequest req)
        {
            var (ok, reason) = await EnsureValidToken(req.service_id, req.token_id);
            if (!ok)
            {
                if (reason == "Token ID expired") return Unauthorized(new { message = reason });
                return BadRequest(new { message = reason });
            }

            var (unsubOk, message) = await _subscriptionService.UnsubscribeAsync(req.service_id, req.phone_number);
            if (!unsubOk) return NotFound(new { message });
            return Ok(new { message });
        }

        [HttpGet("status")]
        public async Task<IActionResult> Status([FromQuery] string service_id, [FromQuery] string token_id, [FromQuery] string phone_number)
        {
            var (ok, reason) = await EnsureValidToken(service_id, token_id);
            if (!ok)
            {
                if (reason == "Token ID expired") return Unauthorized(new { message = reason });
                return BadRequest(new { message = reason });
            }

            var status = await _subscriptionService.CheckStatusAsync(service_id, phone_number);
            return Ok(status);
        }
    }
}
