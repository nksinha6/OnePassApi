using Microsoft.AspNetCore.Mvc;
using OnePass.Domain;
using OnePass.Dto;
using Mapster;

namespace OnePass.API
{
    [ApiController]
    [Route("api/premise/persist")]
    public class PremisePersistController(IPremisePersistService premisePersistService, ILogger<PremisePersistController> logger) : ControllerBase
    {
        private readonly IPremisePersistService _premisePersistService = premisePersistService;
        private readonly ILogger<PremisePersistController> _logger = logger;

        /// <summary>
        /// Creates a new campus entry.
        /// </summary>
        /// <param name="request">Campus DTO payload</param>
        /// <returns>201 Created or appropriate error</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePremise([FromBody] PremiseDto request)
        {
            if (request == null)
            {
                _logger.LogWarning("Received null PremiseDto");

                return BadRequest("Premise data must not be null.");
            }

            try
            {
                var premise = request.Adapt<Premise>();
                var persistedPremise = await _premisePersistService.PersistPremise(premise);

                var locationUrl = Url.Action(
                   action: nameof(PremiseReadController.GetPremiseById),
                   controller: "PremiseRead",
                   values: new { id = persistedPremise.Id },
                   protocol: Request.Scheme);

                _logger.LogInformation("Premise created with Id {Id}.", persistedPremise.Id);

                return Created(locationUrl!, persistedPremise);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while persisting Premise.");
                return StatusCode(500, "An error occurred while creating the Premise.");
            }
        }

        /// <summary>
        /// Creates a new property entry.
        /// </summary>
        /// <param name="request">Property DTO payload</param>
        /// <returns>201 Created or appropriate error</returns>
        [HttpPost("PersistProperty")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateProperty([FromBody] PropertyDto request)
        {
            if (request == null)
            {
                _logger.LogWarning("Received null Property");

                return BadRequest("Property data must not be null.");
            }

            try
            {
                var property = request.Adapt<Property>();
                var persistedProperty = await _premisePersistService.PersistProperty(property);

                var locationUrl = Url.Action(
                   action: nameof(PremiseReadController.GetPremiseById),
                   controller: "PremiseRead",
                   values: new { id = persistedProperty.Id },
                   protocol: Request.Scheme);

                _logger.LogInformation("Premise created with Id {Id}.", persistedProperty.Id);

                return Created(locationUrl!, persistedProperty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while persisting Property.");
                return StatusCode(500, "An error occurred while creating the Property.");
            }
        }

        /// <summary>
        /// Creates a new property entry.
        /// </summary>
        /// <param name="request">Property DTO payload</param>
        /// <returns>201 Created or appropriate error</returns>
        [HttpPost("PersistUnit")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUnit([FromBody] UnitDto request)
        {
            if (request == null)
            {
                _logger.LogWarning("Received null Unit");

                return BadRequest("Unit data must not be null.");
            }

            try
            {
                var unit = request.Adapt<Unit>();
                var persistedUnit = await _premisePersistService.PersistUnit(unit);

                var locationUrl = Url.Action(
                   action: nameof(PremiseReadController.GetPremiseById),
                   controller: "PremiseRead",
                   values: new { id = persistedUnit.Id },
                   protocol: Request.Scheme);

                _logger.LogInformation("Unit created with Id {Id}.", persistedUnit.Id);

                return Created(locationUrl!, persistedUnit);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while persisting Unit.");
                return StatusCode(500, "An error occurred while creating the Unit.");
            }
        }

        /// <summary>
        /// Creates a new desk entry.
        /// </summary>
        /// <param name="request">Desk DTO payload</param>
        /// <returns>201 Created or appropriate error</returns>
        [HttpPost("PersistDesk")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateDesk([FromBody] DeskDto request)
        {
            if (request == null)
            {
                _logger.LogWarning("Received null Desk");

                return BadRequest("Desk data must not be null.");
            }

            try
            {
                var desk = request.Adapt<Desk>();
                var persistedDesk = await _premisePersistService.PersistDesk(desk);

                var locationUrl = Url.Action(
                   action: nameof(PremiseReadController.GetPremiseById),
                   controller: "PremiseRead",
                   values: new { id = persistedDesk.Id },
                   protocol: Request.Scheme);

                _logger.LogInformation("Desk created with Id {Id}.", persistedDesk.Id);

                return Created(locationUrl!, persistedDesk);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while persisting Desk.");
                return StatusCode(500, "An error occurred while creating the Desk.");
            }
        }
    }
}
