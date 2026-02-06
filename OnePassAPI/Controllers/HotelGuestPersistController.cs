using System.ComponentModel.DataAnnotations;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnePass.Domain;
using OnePass.Domain.Services;
using OnePass.Dto;

namespace OnePass.API.Controllers
{
    [ApiController]
    [Route("api/guest/persist")]
    public class HotelGuestPersistController(IHotelGuestPersistService hotelGuestPersistService, 
IOtpService otpService,
ILogger<HotelGuestPersistController> logger) : PersistBaseController
    {
        private readonly IHotelGuestPersistService _hotelGuestPersistService = hotelGuestPersistService;
        private readonly IOtpService _otpService = otpService;
        private readonly ILogger<HotelGuestPersistController> _logger = logger;

        [HttpPost("persist_guest")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> CreateGuest([FromBody] HotelGuestDto request) =>
            ExecutePersistAsync(
                request,
                nameof(HotelGuestReadController.GetGuestById),
                "guest_by_id",
                async () =>
                {
                    var guest = request.Adapt<HotelGuest>();

                    return await _hotelGuestPersistService.Persist(guest);
                });

        [HttpPost("persist_guest_facecapture")]
       // [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> CreateFaceMatch([FromBody] HotelGuestFaceCaptureDto request) =>
            ExecutePersistAsync(
                request,
                nameof(HotelGuestReadController.GetGuestById),
                "guest_by_id",
                async () =>
                {
                    var guestFaceCapture = request.Adapt<HotelGuestFaceCapture>();

                    return await _hotelGuestPersistService.PersistFaceCapture(guestFaceCapture);
                });

        [HttpPost("aadhar/image")]
        [Consumes("multipart/form-data")]
        public Task<IActionResult> PersistGuestAadharImage([FromForm] HotelGuestAadhaarImageDto request) =>
            ExecutePersistAsync(
                request,
                nameof(HotelGuestReadController.GetGuestById),
                "guest_by_id",
                async () =>
                {
                    if (request.Image == null || request.Image.Length == 0)
                        throw new ValidationException("Selfie is required");

                    var entity = request.Adapt<HotelGuestAadhaarImage>();

                    await using var ms = new MemoryStream();
                    await request.Image.CopyToAsync(ms);

                    entity.Image = ms.ToArray();
                    entity.CreatedAt = DateTimeOffset.UtcNow;
                    return await _hotelGuestPersistService.PersistAadharImageAsync(
                entity,
                request.Image.OpenReadStream()
);                });


        [HttpPost("aadhaar/update")]
        // [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> UpdateIdentity([FromBody] UpdateAadharStatusParam request) =>
            ExecutePersistAsync(
                request,
                nameof(HotelGuestReadController.GetGuestById),
                "guest_by_id",
                async () =>
                {
                    return await _hotelGuestPersistService.UpdateAadharData(request);
                });

        [HttpPut("email/update")]
        // [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> UpdateEmail([FromBody] UpdateEmailIdParam request) =>
            ExecutePersistAsync(
                request,
                nameof(HotelGuestReadController.GetGuestById),
                "guest_by_id",
                async () =>
                {
                    return await _hotelGuestPersistService.UpdateEmailIdData(request);
                });

        [HttpPost("sendOtp")]
        // [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> SendOtp([FromBody] SendOtpRequest request) =>
            ExecutePersistAsync(
                request,
                nameof(HotelGuestReadController.GetGuestById),
                "guest_by_id",
                async () =>
                {
                     await _otpService.SendOtpAsync(request.PhoneCountryCode, request.PhoneNumber);

                    return new
                    {
                        Success = true,
                    };
                });

        [HttpGet("delete/guest")]
        // [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> DeleteGuest([FromQuery] string phoneNumber) =>
            ExecutePersistAsync(
                phoneNumber,
                nameof(HotelGuestReadController.GetGuestById),
                "guest_by_id",
                async () =>
                {
                     await _hotelGuestPersistService.DeleteHotelGuest(new DeleteGuestParam()
                    {
                        p_phone_number = phoneNumber
                    });

                    return new
                    {
                        status = $"successfull deleted data for guest {phoneNumber}"
                    };

                });

    }
}
