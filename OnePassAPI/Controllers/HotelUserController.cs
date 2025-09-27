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
    }
}
