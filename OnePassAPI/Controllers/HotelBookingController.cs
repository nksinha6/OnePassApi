using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnePass.API.Controllers;
using OnePass.Domain;
using OnePass.Dto;

namespace OnePass.API
{
    [ApiController]
    [Route("api/booking")]
    [Authorize]
    public class HotelBookingController(IHotelBookingService hotelBookingService,
IRequestContext context, ILogger<HotelBookingController> logger) : PersistBaseController
    {
        IHotelBookingService _hotelBookingService = hotelBookingService;
        private readonly IRequestContext _context = context;

        private readonly ILogger<HotelBookingController> _logger = logger;

        [HttpPost("begin_verification")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> CreateGuest([FromBody] HotelBookingMetadataDto request) =>
            ExecutePersistAsync(
                request.BookingId,
                nameof(HotelGuestReadController.GetGuestById),
                "guest_by_id",
                async () =>
                {
                    var hotelBookingMetadata = request.Adapt<HotelBookingMetadata>();

                    hotelBookingMetadata.TenantId = 1;
                    hotelBookingMetadata.PropertyId = 1;
hotelBookingMetadata.WindowStart = DateTimeOffset.UtcNow;
                    return await _hotelBookingService.StartBookingVerification(hotelBookingMetadata);
                });

        [HttpPost("end_verification")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> EndVerification([FromQuery] string bookingId) =>
            ExecutePersistAsync(
                bookingId,
                nameof(HotelGuestReadController.GetGuestById),
                "guest_by_id",
                async () =>
                {
                    return await _hotelBookingService.EndBookingVerification(_context.TenantId!.Value, _context.PropertyIds.First(), bookingId);
                });

        [HttpPost("face-match/initiate")]
        public Task<IActionResult> InitiateFaceMatch(
        [FromBody] FaceMatchInitiateRequest request)
        =>
            ExecutePersistAsync(
                 request.BookingId,
                 nameof(HotelGuestReadController.GetGuestById),
                 "guest_by_id",
                 async () =>
                 {
                     return await _hotelBookingService.RecordBookingPendingFaceVerification(_context.TenantId!.Value, _context.PropertyIds.First(), request);
                 });
    }
}
