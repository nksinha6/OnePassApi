using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OnePass.Domain;
using OnePass.Dto;
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

        [HttpGet("properties_for_user")]
       // [Authorize]
        public Task<ActionResult<HotelUserPropertiesResponse>> GetUserProperties([FromQuery] string userId) =>
        ExecuteAsync(
            Guid.NewGuid(),
            () => $"user_id_{userId}",
             async () =>
             {
                 var userProperties = await _hotelUserService.GetHotelUserProperties(userId);
                 return userProperties;
             },
            notFoundMessage: $"No user properties found for Id {userId}."
        );
    }
}
