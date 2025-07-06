using Microsoft.AspNetCore.Mvc;
using OnePass.Domain;

namespace OnePass.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class CampusReadController : ControllerBase
    {
        private readonly ICampusReadService _campusReadService;
        private readonly ILogger<CampusReadController> _logger;

        public CampusReadController(ICampusReadService campusReadService,
                                    ILogger<CampusReadController> logger)
        {
            _campusReadService = campusReadService ?? throw new ArgumentNullException(nameof(campusReadService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get campus by Id.
        /// </summary>
        /// <param name="id">Campus Id (Guid) as query parameter</param>
        /// <returns>Campus details</returns>
        [HttpGet]
        public async Task<IActionResult> GetCampusById([FromQuery] Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid Guid provided for campus lookup.");
                return BadRequest("Invalid campus id.");
            }

            try
            {
                var campus = await _campusReadService.GetCampusAsync(new GetCampusByIdQuery() { Id = id });

                if (campus == null)
                {
                    _logger.LogInformation("Campus with Id {Id} not found.", id);
                    return NotFound($"Campus with Id {id} not found.");
                }

                return Ok(campus);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching campus with Id {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
