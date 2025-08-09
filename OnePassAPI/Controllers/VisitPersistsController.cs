using Mapster;
using Microsoft.AspNetCore.Mvc;
using OnePass.Domain;
using OnePass.Dto;

namespace OnePass.API.Controllers
{
    [ApiController]
    [Route("api/user/persist")]
    public class VisitPersistsController(IVisitPersistService visitPersistService, ILogger<VisitPersistsController> logger) : PersistBaseController
    {
        private readonly IVisitPersistService _visitPersistService = visitPersistService;
        private readonly ILogger<VisitPersistsController> _logger = logger;

        [HttpPost("PersistVisitPurpose")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> CreateProperty([FromBody] VisitPurposeDto request) =>
            ExecutePersistAsync(
                request,
                nameof(VisitReadController.GetVisitPurposes),
                "VisitRead",
                async () =>
                {
                    return await _visitPersistService.PersistVisitPurposeAsync(request.Adapt<VisitPurpose>());
                });

        [HttpPost("PersistInvite")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> CreateInvite([FromBody] InviteDto request) =>
            ExecutePersistAsync(
                request,
                nameof(VisitReadController.GetVisitPurposes),
                "VisitRead",
                async () =>
                {
                    return await _visitPersistService.PersistInviteAsync(request);
                });

        [HttpPost("PersistVisit")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> CreateVisit([FromBody] VisitDto request) =>
            ExecutePersistAsync(
                request,
                nameof(VisitReadController.GetVisitPurposes),
                "VisitRead",
                async () =>
                {
                    return await _visitPersistService.PersistVisitAsync(request);
                });

        [HttpPut("UpdateRSVP")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> UpdateRSVP([FromBody] UpdateRSVPParam param) =>
            ExecutePersistAsync(
                param,
                nameof(VisitReadController.GetVisitPurposes),
                "VisitRead",
                async () =>
                {
                    return await _visitPersistService.UpdateRSVPStatus(param);
                });

        [HttpPut("UpdateNDA")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> UpdateNDA([FromBody] UpdateNDAParam param) =>
            ExecutePersistAsync(
                param,
                nameof(VisitReadController.GetVisitPurposes),
                "VisitRead",
                async () =>
                {
                    return await _visitPersistService.UpdateNDAStatus(param);
                });

        [HttpPut("UpdateVisitNDA")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> UpdateVisitNDA([FromBody] UpdateVisitNDAParam param) =>
            ExecutePersistAsync(
                param,
                nameof(VisitReadController.GetVisitPurposes),
                "VisitRead",
                async () =>
                {
                    return await _visitPersistService.UpdateVisitNDAStatus(param);
                });

        [HttpPut("UpdateVisitStatus")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> UpdateVisitStatus([FromBody] UpdateVisitStatusParam param) =>
            ExecutePersistAsync(
                param,
                nameof(VisitReadController.GetVisitPurposes),
                "VisitRead",
                async () =>
                {
                    return await _visitPersistService.UpdateVisitStatus(param);
                });

        [HttpPut("checkin_visit")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> CheckinVisit([FromQuery] Guid visitId) =>
            ExecutePersistAsync(
                "",
                nameof(VisitReadController.GetVisitPurposes),
                "VisitRead",
                async () =>
                {
                    return await _visitPersistService.CheckinVisit(visitId);
                });

        [HttpPut("checkout_visit")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> CheckoutVisit([FromQuery] Guid visitId) =>
            ExecutePersistAsync(
                "",
                nameof(VisitReadController.GetVisitPurposes),
                "VisitRead",
                async () =>
                {
                    return await _visitPersistService.CheckoutVisit(visitId);
                });
    }
}
