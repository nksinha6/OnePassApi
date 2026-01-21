using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NUglify.JavaScript.Syntax;
using OnePass.Domain;
using OnePass.Domain.Services;

namespace OnePass.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelGuestReadController(
    IHotelGuestReadService hotelGuestReadService,
    IHotelGuestAppService hotelGuestAppService,
    ISmsService smsService,
    IOtpService otpService,
ILogger<HotelGuestReadController> logger,
    IMemoryCache cache)
    : ReadControllerBase(logger, cache)
    {
        private readonly IHotelGuestReadService _hotelGuestReadService = hotelGuestReadService;
        private readonly IHotelGuestAppService _hotelGuestAppService = hotelGuestAppService;
        private readonly ISmsService _smsService = smsService;
        private readonly IOtpService _otpService = otpService;

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

        [HttpGet("selfie")]
        // [Authorize]
        public Task<ActionResult<HotelGuestSelfie>> GetSelfieById([FromQuery] string phoneCountryCode, [FromQuery] string phoneno) =>
        ExecuteAsync(
            Guid.NewGuid(),
            () => $"guest_id_{phoneCountryCode}-{phoneno}",
        async () =>
        {
            var guest = await _hotelGuestReadService.GetHotelGuestSelfieAsync(new GetHotelGuestSelfieQuery()
            {
                PhoneCountryCode = phoneCountryCode,
                PhoneNumber = phoneno
            });
            return guest;
        },
            notFoundMessage: $"No user found for Id {phoneCountryCode}-{phoneno}."
        );

        [HttpGet("verification/ensure")]
        // [Authorize]
        public Task<ActionResult<HotelGuestResponse>> EnsureVerification([FromQuery] string phoneCountryCode, [FromQuery] string phoneno) =>
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

            if(guest.VerificationStatus != VerificationStatus.verified)
            {
                //send sms
                await _smsService.SendOnboardingLinkSmsAsync(phoneCountryCode,  phoneno);
            }

            return guest;
        },
            notFoundMessage: $"No user found for Id {phoneCountryCode}-{phoneno}."
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
    }
}
