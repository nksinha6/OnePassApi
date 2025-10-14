using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OnePass.Domain;
using OnePass.Dto.Response;

namespace OnePass.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelUserReadController(
    IHotelUserService hotelUserService,
    ILogger<HotelUserReadController> logger,
    IMemoryCache cache)
    : ReadControllerBase(logger, cache)
    {
        IHotelUserService _hotelUserService = hotelUserService;
        [HttpGet("user_by_id")]
        [Authorize]
        public Task<ActionResult<HotelUserResponse>> GetUnitsByCompanyId([FromQuery] string userId, [FromQuery] int tenantId) =>
        ExecuteAsync(
            Guid.NewGuid(),
            () => $"user_id_{userId}",
             async () =>
             {
                 var user = await _hotelUserService.GetUser(userId, tenantId);
                 return user;
             },
            notFoundMessage: $"No user found for Id {userId}."
        );
    }
}
