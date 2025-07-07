using Microsoft.AspNetCore.Mvc;
using OnePass.Domain;
using OnePass.Dto;
using Mapster;

namespace OnePass.API
{
    [ApiController]
    [Route("api/campus/persist")]
    public class CampusPersistController(ICampusPersistService campusPersistService, ILogger<CampusPersistController> logger) : ControllerBase
    {
        private readonly ICampusPersistService _campusPersistService = campusPersistService;
        private readonly ILogger<CampusPersistController> _logger = logger;

        /// <summary>
        /// Creates a new campus entry.
        /// </summary>
        /// <param name="request">Campus DTO payload</param>
        /// <returns>201 Created or appropriate error</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCampus([FromBody] CampusDto request)
        {
            if (request == null)
            {
                _logger.LogWarning("Received null CampusDto");
                return BadRequest("Campus data must not be null.");
            }

            try
            {
                var campus = request.Adapt<Campus>();
                var persistedCampus = await _campusPersistService.PersistCampus(campus);

                var locationUrl = Url.Action(
           action: nameof(CampusReadController.GetCampusById),
           controller: "CampusRead",
           values: new { id = persistedCampus.Id },
           protocol: Request.Scheme);

                _logger.LogInformation("Campus created with Id {Id}.", persistedCampus.Id);

                return Created(locationUrl!, persistedCampus);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while persisting campus.");
                return StatusCode(500, "An error occurred while creating the campus.");
            }
        }
    }
}
