using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OnePass.API.Controllers;
using OnePass.Domain;
using OnePass.Domain.Services;

namespace OnePass.API
{
    public class HotelBookingReadControl(
        IHotelBookingReadService hotelBookingReadService,
        ILogger<HotelGuestReadController> logger,
    IMemoryCache cache)
    : ReadControllerBase(logger, cache)
    {

        private readonly IHotelBookingReadService _hotelBookingReadService = hotelBookingReadService;

        [HttpGet("pending_face_matches")]
        // [Authorize]
        public Task<ActionResult<IEnumerable<HotelPendingFaceMatchDetailedResponse>>> GetPendingFaceMatches()
    => ExecuteAsync(
        Guid.NewGuid(),
        () => $"pending_face_matches",
        async () =>
        {
            var result = await _hotelBookingReadService.GetPendingFaceMatchesAsync(1, 1);

            return result;
        },
        notFoundMessage: $"No pending face match reservations found for tenantId 1 and propertyId 1."
    );

        [HttpPost("face-match/status")]
        public Task<ActionResult<HotelPendingFaceMatchResponse>> GetFaceMatchStatus(
    [FromBody] FaceMatchStatusRequest request)
        => ExecuteAsync(
        Guid.NewGuid(),
        () => $"pending_face_matches",
        async () =>
        {
            var result = await _hotelBookingReadService.GetFaceMatchStatusAsync(new GetFaceMatchByBookingAndPhoneQuery()
            {
                TenantId = 1,
                PropertyId = 1,
                BookingId = request.BookingId,
                PhoneCountryCode = request.PhoneCountryCode,
                PhoneNumber = request.PhoneNumber

            });

            return result;
        },
        notFoundMessage: $"No pending face match reservations found for tenantId 1 and propertyId 1."
    );

    }
}
