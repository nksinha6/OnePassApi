using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OnePass.Domain;
using OnePass.Domain.Services;
using OnePass.Dto;

namespace OnePass.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelPropertyReadController(
IHotelPropertyReadService hotelPropertyReadService,        ILogger<HotelPropertyReadController> logger,
    IMemoryCache cache)
    : ReadControllerBase(logger, cache)
    {
        private readonly IHotelPropertyReadService _hotelPropertyReadService = hotelPropertyReadService;

        [HttpGet("property_by_id")]
        public Task<ActionResult<HotelPropertyNameResponse>> GetHotelById([FromQuery] int propertyId)
    => ExecuteAsync(
        Guid.NewGuid(),
        () => $"property_by_id",
        async () =>
        {
            var result = await _hotelPropertyReadService.GetPropertyNameAsync(propertyId);
            return result;
        },
        notFoundMessage: $"No properties found."
        );

        [HttpGet("tenant_by_id")]
        public Task<ActionResult<HotelTenantResponse>> GetTenantById([FromQuery] int tenantId)
    => ExecuteAsync(
        Guid.NewGuid(),
        () => $"tenant_by_id",
        async () =>
        {
            var result = await _hotelPropertyReadService.GetTenantAsync(tenantId);
            return result;
        },
        notFoundMessage: $"No tenant found."
        );
    }
}
