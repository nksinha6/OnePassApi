using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OnePass.Domain;
using OnePass.Domain.Services;

namespace OnePass.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelGuestReadController(
    IHotelGuestReadService hotelGuestReadService,
    ILogger<HotelGuestReadController> logger,
    IMemoryCache cache)
    : ReadControllerBase(logger, cache)
    {
        private readonly IHotelGuestReadService _hotelGuestReadService = hotelGuestReadService;

        [HttpGet("guest_by_id")]
        [Authorize]
        public Task<ActionResult<HotelGuestResponse>> GetGuestById([FromQuery] string phoneCountryCode, [FromQuery] string phoneno) =>
        ExecuteAsync(
            Guid.NewGuid(),
            () => $"guest_id_{phoneCountryCode}-{phoneno}",
        async () =>
        {
                 var guest = await _hotelGuestReadService.GetHotelGuestAsync(new GetHotelGuestByPhoneQuery()
                 {
                     PhoneCountryCode = phoneCountryCode,
                     PhoneNumber = phoneno
                 });
                 return guest;
             },
            notFoundMessage: $"No user found for Id {phoneCountryCode}-{phoneno}."
        );
    }
}
