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
    public class HotelBookingReadController(
        IHotelBookingReadService hotelBookingReadService,
        IHotelGuestReadService
            hotelGuestReadService,
        IFaceVerificationService
        faceVerificationService, 
IHotelBookingService hotelBookingService,        
ILogger<HotelGuestReadController> logger,
    IMemoryCache cache)
    : ReadControllerBase(logger, cache)
    {

        private readonly IHotelBookingReadService _hotelBookingReadService = hotelBookingReadService;
        private readonly IHotelGuestReadService
            _hotelGuestReadService = hotelGuestReadService;
        private readonly IFaceVerificationService _faceVerificationService = faceVerificationService;
        private readonly IHotelBookingService _hotelBookingService = hotelBookingService;

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

        [HttpPost("selfie_match")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> MatchBookingGuestSelfie([FromForm] HotelBookingGuestSelfieMatchRequestDto request)
        {
            var selfie = await _hotelGuestReadService.GetHotelGuestSelfieAsync(new GetHotelGuestSelfieQuery()
            {
               PhoneCountryCode = request.PhoneCountryCode,
               PhoneNumber = request.PhoneNumber
            });

            if(selfie == null)
                return NotFound();
            ImageInput selfieImage = new ImageInput(
            Stream: new MemoryStream(selfie.Image, writable: false),
            FileName: "selfie.jpg",
            ContentType: selfie.ContentType,
            Length: selfie.Image.LongLength
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
            await _hotelBookingService.VerifyBookingPendingFaceVerification(request.Id);
            
            return Ok(new 
            {
                Result = verificationResult
            });
        }
    }
}
