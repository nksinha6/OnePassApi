using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnePass.API.Controllers;
using OnePass.Domain;

namespace OnePass.API
{
    [ApiController]
    [Route("api/booking")]
    public class HotelBookingController(IHotelBookingService hotelBookingService, ILogger<HotelBookingController> logger) : PersistBaseController
    {
        IHotelBookingService _hotelBookingService = hotelBookingService;

        private readonly ILogger<HotelBookingController> _logger = logger;

        [HttpPost("begin_verification")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> CreateGuest([FromQuery] string bookingId) =>
            ExecutePersistAsync(
                bookingId,
                nameof(HotelGuestReadController.GetGuestById),
                "guest_by_id",
                async () =>
                {
                    return await _hotelBookingService.StartBookingCheckin(1, bookingId);
                });

        [HttpPost("face-match/initiate")]
        public async Task<IActionResult> InitiateFaceMatch(
        [FromBody] FaceMatchInitiateRequest request)
        {
            // TODO: Implement logic

            return Ok(new
            {
               isInitiated = true
            });
        }


        [HttpPost("record_checkin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> RecordCheckin([FromQuery] string bookingId, [FromQuery] int tenantId) =>
            ExecutePersistAsync(
                bookingId,
                nameof(HotelGuestReadController.GetGuestById),
                "guest_by_id",
                async () =>
                {
                    return await _hotelBookingService.RecordBookingCheckin(tenantId, bookingId);
                });

        [HttpGet("face-match/status")]
        public async Task<IActionResult> GetFaceMatchStatus(
    [FromBody] FaceMatchStatusRequest request)
        {
            /*  var response = await _faceMatchService.GetFaceMatchStatus(
                  request.BookingId,
                  request.PhoneCountryCode,
                  request.PhoneNumber);
            */
            var response = new FaceMatchStatusResponse
            {
                BookingId = request.BookingId,
                PhoneCountryCode = request.PhoneCountryCode,
                PhoneNumber = request.PhoneNumber,
                IsFaceMatched = true,
                FaceMatchScore = 87.3
            };

            return Ok(response);
        }

    }
}
