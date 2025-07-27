using Microsoft.AspNetCore.Mvc;
using OnePass.Domain;
using OnePass.Dto;
using Mapster;

namespace OnePass.API
{
    [ApiController]
    [Route("api/premise/persist")]
    public class PremisePersistController : PersistBaseController
    {
        private readonly IPremisePersistService _premisePersistService;
        private readonly ILogger<PremisePersistController> _logger;

        public PremisePersistController(
            IPremisePersistService premisePersistService,
            ILogger<PremisePersistController> logger)
        {
            _premisePersistService = premisePersistService;
            _logger = logger;
        }

        [HttpPost("PersistProperty")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> CreateProperty([FromBody] PropertyDto request) =>
            ExecutePersistAsync(
                request,
                nameof(PremiseReadController.GetPropertyByCompanyId),
                "PremiseRead",
                async () =>
                {
                    var property = request.Adapt<Property>();
                    return await _premisePersistService.PersistProperty(property);
                });

        [HttpPost("PersistUnit")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> CreateUnit([FromBody] UnitDto request) =>
            ExecutePersistAsync(
                request,
                nameof(PremiseReadController.GetUnitsByCompanyId),
                "PremiseRead",
                async () =>
                {
                    var unit = request.Adapt<Unit>();
                    return await _premisePersistService.PersistUnit(unit);
                });

        [HttpPost("PersistDesk")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> CreateDesk([FromBody] DeskDto request) =>
            ExecutePersistAsync(
                request,
                nameof(PremiseReadController.GetDesksByCompanyId),
                "PremiseRead",
                async () =>
                {
                    var desk = request.Adapt<Desk>();
                    return await _premisePersistService.PersistDesk(desk);
                });
    }
}
