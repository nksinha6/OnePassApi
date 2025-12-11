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
    IHotelGuestAppService hotelGuestAppService,
    ILogger<HotelGuestReadController> logger,
    IMemoryCache cache)
    : ReadControllerBase(logger, cache)
    {
        private readonly IHotelGuestReadService _hotelGuestReadService = hotelGuestReadService;
        private readonly IHotelGuestAppService _hotelGuestAppService = hotelGuestAppService;

        [HttpGet("guest_by_id")]
       // [Authorize]
        public Task<ActionResult<HotelGuestResponse>> GetGuestById([FromQuery] string phoneCountryCode, [FromQuery] string phoneno) =>
        ExecuteAsync(
            Guid.NewGuid(),
            () => $"guest_id_{phoneCountryCode}-{phoneno}",
        async () =>
        {
            if (!string.IsNullOrEmpty(phoneCountryCode))
            {
                // Replace space with plus if it was decoded
                phoneCountryCode = phoneCountryCode.Replace(" ", "+");
            }

            var guest = await _hotelGuestAppService.GetForCreateIfNotExists(new GetHotelGuestByPhoneQuery()
                 {
                     PhoneCountryCode = phoneCountryCode,
                     PhoneNumber = phoneno
                 });
                 return guest;
             },
            notFoundMessage: $"No user found for Id {phoneCountryCode}-{phoneno}."
        );

        [HttpGet("pending_face_matches")]
        // [Authorize]
        public Task<ActionResult<IEnumerable<HotelPendingFaceMatchResponse>>> GetPendingFaceMatches()
    => ExecuteAsync(
        Guid.NewGuid(),
        () => $"pending_face_matches",
        async () =>
        {
            var result = await _hotelGuestReadService.GetPendingFaceMatchesAsync(1, 1);

            return result;
        },
        notFoundMessage: $"No pending face match reservations found for tenantId 1 and propertyId 1."
    );
    }
}
