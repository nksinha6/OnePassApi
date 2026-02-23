using Mapster;
using Microsoft.AspNetCore.Mvc;
using OnePass.Domain;
using OnePass.Dto;
using OnePass.Dto.Request.Hotel;

namespace OnePass.API
{
    [ApiController]
    [Route("api/premise/persist")]
    public class HotelPropertiesPersistController : PersistBaseController
    {
        private readonly IHotelPropertiesPersistService _propertiesPersistService;
        private readonly ILogger<HotelPropertiesPersistController> _logger;

        public HotelPropertiesPersistController(
            IHotelPropertiesPersistService propertiesPersistService,
            ILogger<HotelPropertiesPersistController> logger)
        {
            _propertiesPersistService = propertiesPersistService;
            _logger = logger;
        }

        [HttpPost("PersistTenant")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> CreateTenant([FromForm] HotelTenantRequestDto request) =>
            ExecutePersistAsync(
                request,
                nameof(HotelPropertyReadController.GetTenantById),
                "tenant_by_id",
                async () =>
                {
                    var tenant = request.Adapt<HotelTenant>();
                    return await _propertiesPersistService.PersistTenantAsync(tenant);
                });
    }
}
