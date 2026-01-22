using Microsoft.AspNetCore.Mvc;
using OnePass.Domain;
using OnePass.Dto;
using Mapster;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using Azure;

namespace OnePass.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelUserController(
    IHotelUserService hotelUserService,
    IRequestContext context,
    ILogger<HotelUserController> logger): PersistBaseController
    {
        private readonly IHotelUserService 
            _hotelUserService = hotelUserService;
        private readonly IRequestContext _context = context;

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
        [AllowAnonymous]
        public async Task<IActionResult> Login(
    [FromBody] HotelLoginRequest request,
    [FromServices] IJwtService jwtService,
    [FromServices] IHasher passwordHasher,
    [FromServices] IRefreshTokenService refreshTokenService) // service to generate/store refresh token
        {
            var passwordRecord = await _hotelUserService.GetPassword(request.UserId);
            if (passwordRecord == null)
                return BadRequest("User does not exist.");

            if (!passwordHasher.Verify(request.Password, passwordRecord.PasswordHash))
                return Unauthorized("Invalid credentials.");

            var user = await _hotelUserService.GetUser(request.UserId);
            var userProperties =
                await _hotelUserService.GetHotelUserProperties(request.UserId);
            // Generate access token
            var accessToken = jwtService.GenerateToken(request.UserId, userProperties.TenantId, userProperties.Properties
                          .Select(p => p.PropertyId)
                          .ToList(), user.Role);

            // Generate refresh token
            var refreshToken = await refreshTokenService.CreateRefreshToken(request.UserId, userProperties.TenantId);

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

            var newAccessToken = jwtService.GenerateToken(request.UserId, request.TenantId, request.PropertyIds, request.Role);

            return Ok(new { AccessToken = newAccessToken });
        }

        [HttpPost("user_property")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PersistUserProperty([FromBody] HotelUserProperty request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.UserId))
                return BadRequest("UserId is required.");

            await _hotelUserService.AddUserProperty(request);

            return Ok("Hotel user property added successfully.");

        }
    }
}
