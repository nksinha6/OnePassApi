using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NUglify.JavaScript.Syntax;
using OnePass.Domain;
using OnePass.Domain.Services;
using OnePass.Dto;

namespace OnePass.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelGuestReadController(
    IHotelGuestReadService hotelGuestReadService,
    IHotelGuestAppService hotelGuestAppService,
    ISmsService smsService,
    IOtpService otpService,
    IRequestContext requestContext,
ILogger<HotelGuestReadController> logger,
    IMemoryCache cache)
    : ReadControllerBase(logger, cache)
    {
        private readonly IHotelGuestReadService _hotelGuestReadService = hotelGuestReadService;
        private readonly IHotelGuestAppService _hotelGuestAppService = hotelGuestAppService;
        private readonly ISmsService _smsService = smsService;
        private readonly IOtpService _otpService = otpService;

        private readonly IRequestContext _requestContext = requestContext;

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

        [HttpGet("aadhar/image")]
        // [Authorize]
        public Task<ActionResult<HotelGuestAadhaarImage>> GetAadharById([FromQuery] string phoneCountryCode, [FromQuery] string phoneno) =>
        ExecuteAsync(
            Guid.NewGuid(),
            () => $"guest_id_{phoneCountryCode}-{phoneno}",
        async () =>
        {
            var guest = await _hotelGuestReadService.GetHotelGuestAadharImageAsync(new GetHotelGuestAadharImageQuery()
            {
                PhoneCountryCode = phoneCountryCode,
                PhoneNumber = phoneno
            });
            return guest;
        },
            notFoundMessage: $"No user found for Id {phoneCountryCode}-{phoneno}."
        );

        [HttpPost("verification/ensure")]
        [Authorize]
        public Task<ActionResult<HotelGuestResponse>> EnsureVerification([FromBody] HotelEnsureGuestRequestDto request) =>
        ExecuteAsync(
            Guid.NewGuid(),
            () => $"guest_id_{request.PhoneCountryCode}-{request.PhoneNumber}",
        async () =>
        {
            if (!string.IsNullOrEmpty(request.PhoneCountryCode))
            {
                // Replace space with plus if it was decoded
                request.PhoneCountryCode = request.PhoneCountryCode.Replace(" ", "+");
            }

            var guest = await _hotelGuestAppService.GetForCreateIfNotExists(new GetHotelGuestByPhoneQuery()
            {
                PhoneCountryCode = request.PhoneCountryCode,
                PhoneNumber = request.PhoneNumber
            });

             await _smsService.SendOnboardingLinkSmsAsync(request.PhoneCountryCode, request.PhoneNumber, _requestContext.PropertyIds.First());
 
            await _hotelGuestAppService.AddBookingGyest(new HotelBookingGuest()
            {
                TenantId = _requestContext.TenantId!.Value,
                PropertyId = _requestContext.PropertyIds.First(),
                PhoneCountryCode = request.PhoneCountryCode,
                PhoneNumber = request.PhoneNumber,
                BookingId = request.BookingId,
                CreatedAt = DateTime.UtcNow,
            });

            return guest;
        },
            notFoundMessage: $"No user found for Id {request.PhoneCountryCode}-{request.PhoneNumber}."
        );

        [HttpPost("verify_otp")]
        // [Authorize]
        public Task<ActionResult<HotelGuestVerifyOtpResponse>> VerifyOtp([FromBody] HotelGuestVerifyOtpRequest request) => 
          ExecuteAsync(
            Guid.NewGuid(),
            () => $"guest_id_{request.PhoneCountryCode}-{request.PhoneNumber}",
        async () =>
        {
            return await _otpService.VerifyOtpAsync(
                  request.PhoneCountryCode,
                  request.PhoneNumber,
                  request.Otp
              );

            
        },
            notFoundMessage: $"No otp found for Id {request.PhoneCountryCode}-{request.PhoneCountryCode}."
        );

        [HttpPost("pending_qrcode_phone_number")]
        public Task<ActionResult<PendingQrCodeMatchesByPhoneResponse>> PendingQRCodeByPhoenNumber([FromBody] GetPendingQrCodeMatchesByPhoneQuery request) =>
          ExecuteAsync(
            Guid.NewGuid(),
            () => $"pending_qr_code{request.PhoneCountryCode}-{request.PhoneNumber}",
        async () =>
        {
            return await _hotelGuestReadService.GetPendingQrCodeMatchesByPhoneResponseAsync( request );


        },
            notFoundMessage: $"No pending qr code found for {request.PhoneCountryCode}-{request.PhoneCountryCode}."
        );
    }
}
