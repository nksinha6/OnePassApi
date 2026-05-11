using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using OnePass.Domain;

namespace OnePass.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class KycController(IAzapiService azapiService,
    ILogger<KycController> logger,
    IMemoryCache cache)
    : ReadControllerBase(logger, cache)
    {
        private readonly IAzapiService _azapiService = azapiService;

        [HttpPost("passport")]
        public async Task<IActionResult> UploadPassport(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file");

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);

            var result = await _azapiService.ExtractPassportAsync(ms.ToArray());

            return Ok(result);
        }
    }
}
