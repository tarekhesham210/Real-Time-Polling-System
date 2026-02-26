using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
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
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterRequestDTO request)
        {
            var result = await _authService.RegisterAsync(request);
            if(!result.IsSuccess)
                return StatusCode((int)result.StatusCode, result);
            return StatusCode((int)result.StatusCode,result.Value);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequestDTO request)
        {
            var result = await _authService.LoginAsync(request);
            if(!result.IsSuccess)
                return StatusCode((int)result.StatusCode, result);
            return StatusCode((int)result.StatusCode,result.Value);
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest refreshToken)
        {
            var result = await _authService.RefreshTokenAsync(refreshToken);
            if (!result.IsSuccess) 
                return StatusCode((int)result.StatusCode, result);

            return StatusCode((int)result.StatusCode,result.Value);
        }
        [Authorize] 
        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken(RefreshTokenRequest request)
        {
           
            var result = await _authService.RevokeTokenAsync(request);
            if (!result.IsSuccess)
                return StatusCode((int)result.StatusCode,result);

            return StatusCode((int)result.StatusCode);
        }

      
    }
}
