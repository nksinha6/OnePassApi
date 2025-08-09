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

        [HttpPost("persist_visit_purpose")]
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

        [HttpPost("persist_invite")]
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

        [HttpPost("persist_visit")]
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

        [HttpPut("update_RSVP")]
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

        [HttpPut("update_NDA")]
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

        [HttpPut("update_visit_NDA")]
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

        [HttpPut("update_visit_approval_status")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> UpdateVisitApprovalStatus([FromBody] UpdateVisitApprovalStatusParam param) =>
            ExecutePersistAsync(
                param,
                nameof(VisitReadController.GetVisitPurposes),
                "VisitRead",
                async () =>
                {
                    return await _visitPersistService.UpdateVisitApprovalStatus(param);
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
