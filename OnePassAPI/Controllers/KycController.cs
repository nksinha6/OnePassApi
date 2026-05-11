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
        public async Task<IActionResult> UploadPassport(IFormFile front, IFormFile back)
        {
            if (front == null || front.Length == 0)
                return BadRequest("Invalid file");
            if (back == null || back.Length == 0)
                return BadRequest("Invalid file");

            using var ms = new MemoryStream();
            await front.CopyToAsync(ms);

            var result = await _azapiService.ExtractPassportAsync(ms.ToArray());
            using var ms2 = new MemoryStream();
            await back.CopyToAsync(ms2);

            var result2 = await _azapiService.ExtractPassportAsync(ms2.ToArray());
            result.address = result2.address;
            return Ok(result);
        }
    }
}
