using Microsoft.AspNetCore.Mvc;
using OnePass.Domain;
using OnePass.Dto;
using Mapster;

namespace OnePass.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelUserController(
    IHotelUserService hotelUserService,
    ILogger<HotelUserController> logger): PersistBaseController
    {
        private readonly IHotelUserService 
            _hotelUserService = hotelUserService;

        [HttpPost("PersistPassword")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PersistPassword([FromBody] HotelUserPasswordDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.UserId) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("UserId and Password are required.");

            await _hotelUserService.SetPassword(request.UserId, request.Password, request.TenantId);

            return Ok("Password updated successfully.");

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
    [FromBody] HotelLoginRequest request,
    [FromServices] IJwtService jwtService,
    [FromServices] IPasswordHasher passwordHasher,
    [FromServices] IRefreshTokenService refreshTokenService) // service to generate/store refresh token
        {
            var passwordRecord = await _hotelUserService.GetPassword(request.UserId, request.TenantId);
            if (passwordRecord == null)
                return BadRequest("User does not exist.");

            if (!passwordHasher.VerifyPassword(request.Password, passwordRecord.PasswordHash))
                return Unauthorized("Invalid credentials.");

            // Generate access token
            var accessToken = jwtService.GenerateToken(request.UserId, request.TenantId, "User");

            // Generate refresh token
            var refreshToken = await refreshTokenService.CreateRefreshToken(request.UserId, request.TenantId);

            return Ok(new
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                ExpiresAt = refreshToken.ExpiresAt
            });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request,
    [FromServices] IJwtService jwtService,
    [FromServices] IRefreshTokenService refreshTokenService)
        {
            var isValid = await refreshTokenService.ValidateRefreshToken(request.Token, request.UserId, request.TenantId);
            if (!isValid)
                return Unauthorized("Invalid refresh token.");

            var newAccessToken = jwtService.GenerateToken(request.UserId, request.TenantId, "User");

            return Ok(new { AccessToken = newAccessToken });
        }
    }
}
