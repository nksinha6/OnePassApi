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
   // [Authorize]
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

                    hotelBookingMetadata.TenantId = _context.TenantId!.Value;
                    hotelBookingMetadata.PropertyId = _context.PropertyIds.First();
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

        [HttpPost("qr-code/initiate")]
        public Task<IActionResult> InitiateQRMatch(
        [FromBody] PendingQRCodeMatchRequest request)
        =>
            ExecutePersistAsync(
                 request.BookingId,
                 nameof(HotelGuestReadController.GetGuestById),
                 "guest_by_id",
                 async () =>
                 {
                     var pendingQRCode = request.Adapt<HotelPendingQrCodeMatch>();
                     pendingQRCode.TenantId = 2; // _context.TenantId!.Value;
                     pendingQRCode.PropertyId = 3;// _context.PropertyIds.First();
                     return await _hotelBookingService.RecordHotelPendingQrCodeMatch(pendingQRCode);
                 });

        [HttpPut("qr-code/verify")]
        public Task<IActionResult> VerifyQRMatch(
        [FromQuery] int id)
        =>
            ExecutePersistAsync(
                 id,
                 nameof(HotelGuestReadController.GetGuestById),
                 "qrcode_by_phone",
                 async () =>
                 {
                    return await _hotelBookingService.VerifyHotelPendingQrCodeMatch(id);
                 });
    }
}
