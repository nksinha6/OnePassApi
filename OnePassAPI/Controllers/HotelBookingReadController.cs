using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OnePass.API.Controllers;
using OnePass.Domain;
using OnePass.Domain.Services;
using OnePass.Dto;

namespace OnePass.API
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class HotelBookingReadController(
        IHotelBookingReadService hotelBookingReadService,
        IHotelGuestReadService
            hotelGuestReadService,
        IFaceVerificationService
        faceVerificationService, 
IHotelBookingService hotelBookingService,
IRequestContext context,
ILogger<HotelGuestReadController> logger,
    IMemoryCache cache)
    : ReadControllerBase(logger, cache)
    {

        private readonly IHotelBookingReadService _hotelBookingReadService = hotelBookingReadService;
        private readonly IHotelGuestReadService
            _hotelGuestReadService = hotelGuestReadService;
        private readonly IFaceVerificationService _faceVerificationService = faceVerificationService;
        private readonly IHotelBookingService _hotelBookingService = hotelBookingService;

        private readonly IRequestContext _context = context;

        [HttpGet("pending_face_matches")]
        // [Authorize]
        public Task<ActionResult<IEnumerable<HotelPendingFaceMatchDetailedResponse>>> GetPendingFaceMatches()
    => ExecuteAsync(
        Guid.NewGuid(),
        () => $"pending_face_matches",
        async () =>
        {
            var result = await _hotelBookingReadService.GetPendingFaceMatchesAsync(_context.TenantId!.Value, _context.PropertyIds[0]);

            return result;
        },
        notFoundMessage: $"No pending face match reservations found."
    );


        [HttpGet("pending_qrcode_matches")]
        public Task<ActionResult<PendingQrCodeMatchesResponse>> GetPendingQRCodeMatches()
    => ExecuteAsync(
        Guid.NewGuid(),
        () => $"pending_qrcode_matches",
        async () =>
        {
            var result = await _hotelBookingReadService.GetPendingQrCodeMatchesResponseAsync(new GetPendingQrCodeMatchesQuery()
            {
                TenantId = _context.TenantId!.Value,
                PropertyId = _context.PropertyIds.First()
            });

            return result;
        },
        notFoundMessage: $"No pending qr code found."
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
                TenantId = _context.TenantId!.Value,
                PropertyId = _context.PropertyIds.First(),
                BookingId = request.BookingId,
                PhoneCountryCode = request.PhoneCountryCode,
                PhoneNumber = request.PhoneNumber

            });

            return result;
        },
        notFoundMessage: $"No pending face match reservations found."
    );

        [HttpPost("selfie_match")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> MatchBookingGuestSelfie([FromForm] HotelBookingGuestSelfieMatchRequestDto request)
        {
            var selfie = await _hotelGuestReadService.GetHotelGuestAadharImageAsync(new GetHotelGuestAadharImageQuery()
            {
               PhoneCountryCode = request.PhoneCountryCode,
               PhoneNumber = request.PhoneNumber
            });

            if(selfie == null)
                return NotFound();
            byte[] uploadedSelfieBytes;
            using (var ms = new MemoryStream())
            {
                await request.Selfie.CopyToAsync(ms);
                uploadedSelfieBytes = ms.ToArray();
            }

            ImageInput selfieImage = new ImageInput(
    Stream: new MemoryStream(uploadedSelfieBytes, writable: false),
    FileName: Path.GetFileName(request.Selfie.FileName),
    ContentType: string.IsNullOrWhiteSpace(request.Selfie.ContentType)
        ? "application/octet-stream"
        : request.Selfie.ContentType,
    Length: uploadedSelfieBytes.LongLength
);
            var inputImage = new ImageInput(
            Stream: request.Selfie.OpenReadStream(),
            FileName: Path.GetFileName(request.Selfie.FileName),
            ContentType: string.IsNullOrWhiteSpace(request.Selfie.ContentType)
                ? "application/octet-stream"
                : request.Selfie.ContentType,
            Length: request.Selfie.Length
        );


            var verificationResult = await _faceVerificationService.MatchFacesAsync(Guid.NewGuid().ToString(), selfieImage, inputImage);

            if(verificationResult == null) return NotFound();
         
            if(verificationResult.FaceMatchResult.ToLower() == "yes")
            await _hotelBookingService.VerifyBookingPendingFaceVerification(request.BookingId, request.Id, uploadedSelfieBytes,
    selfieImage.ContentType,
    uploadedSelfieBytes.LongLength, request.Latitude, request.Longitude, request.PhoneCountryCode, request.PhoneNumber,
    _context.TenantId!.Value, _context.PropertyIds.First());
            
            return Ok(new 
            {
                Result = verificationResult
            });
        }

        [HttpGet("all_booking")]
        // [Authorize]
        public Task<ActionResult<IEnumerable<HotelBookingMetadataResponse>>> GetAllBooking() =>
        ExecuteAsync(
            Guid.NewGuid(),
            () => $"all_booking",
        async () =>
        {
            var bookings = await _hotelBookingReadService.GeHotelMetadataAsync(new GetHotelBookingMetadataQuery()
            {
                TenantId = _context.TenantId!.Value,
                PropertyId = _context.PropertyIds.First(),
            });
            return bookings;
        },
            notFoundMessage: $"No user found for Id {1}-{1}."
        );
    }
}
