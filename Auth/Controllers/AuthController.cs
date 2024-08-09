using Microsoft.AspNetCore.Mvc;
using Z1.Auth.Dtos;
using Z1.Auth.Interfaces;
using Z1.Core;

namespace Z1.Auth.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto model)
        {
            var response = await _authService.Login(model, ipAddress());
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp(SendOtpRequestDto model)
        {
            var response = await _authService.SendOtp(model);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(VerifyOtpRequestDto model)
        {
            return Ok(await _authService.VerifyOtp(model, ipAddress()));
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public IActionResult RefreshToken(RefreshRequestDto model)
        {
            var response = _authService.RefreshToken(model.Refresh, ipAddress());
            return Ok(response);
        }

        [HttpPost("revoke-token")]
        public IActionResult RevokeToken(RefreshRequestDto model)
        {
            if (string.IsNullOrEmpty(model.Refresh))
                return BadRequest(new { message = "Token is required" });

            _authService.RevokeToken(model.Refresh, ipAddress());
            return Ok(new { message = "Token revoked" });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _authService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _authService.GetById(id);
            return Ok(user);
        }

        [HttpGet("{id}/refresh-tokens")]
        public IActionResult GetRefreshTokens(int id)
        {
            var user = _authService.GetById(id);
            return Ok(user.RefreshTokens);
        }

        // helper methods

        private string ipAddress()
        {
            // get source ip address for the current request
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
