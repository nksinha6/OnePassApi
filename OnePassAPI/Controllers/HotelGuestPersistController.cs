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
    public class HotelGuestPersistController(IHotelGuestPersistService hotelGuestPersistService, ILogger<HotelGuestPersistController> logger) : PersistBaseController
    {
        private readonly IHotelGuestPersistService _hotelGuestPersistService = hotelGuestPersistService;

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

        [HttpPost("persist_guest_selfie")]
        public Task<IActionResult> PersistGuestSelfie([FromBody] HotelGuestSelfie request) =>
            ExecutePersistAsync(
                request,
                nameof(HotelGuestReadController.GetGuestById),
                "guest_by_id",
                async () =>
                {
                    var guest = request.Adapt<HotelGuest>();

                    return await _hotelGuestPersistService.PersistSelfie(request);
                });
    }
}
