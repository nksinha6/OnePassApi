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
    }
}
