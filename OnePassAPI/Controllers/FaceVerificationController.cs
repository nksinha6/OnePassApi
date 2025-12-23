using Microsoft.AspNetCore.Mvc;
using OnePass.Domain;

namespace OnePass.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class FaceVerificationController : ControllerBase
    {
        private readonly IFaceVerificationService _faceService;
        private readonly ILogger<FaceVerificationController> _logger;

        public FaceVerificationController(IFaceVerificationService faceService, ILogger<FaceVerificationController> logger)
        {
            _faceService = faceService;
            _logger = logger;
        }

        [HttpPost("liveness")]
        public IActionResult FaceMatch(
    [FromForm] string countryCode,
    [FromForm] string phoneNumber,
    [FromForm] string bookingId,
    [FromForm] IFormFile selfieImage)
        {
            if (selfieImage == null || selfieImage.Length == 0)
                return BadRequest("Selfie image is required");

            return Ok(new
            {
                faceVerified = true,
                faceMatchScore = 93.2
            });
        }


        [HttpPost("face-match")]
        public async Task<IActionResult> Liveness([FromForm] string verificationId, [FromForm] IFormFile selfie)
        {
            if (string.IsNullOrEmpty(verificationId) || selfie == null)
                return BadRequest("verificationId and selfie required");

            try
            {
                var result = await _faceService.CheckLivenessAsync(verificationId, selfie);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Liveness check");
                return StatusCode(500, new { error = "Face liveness failed" });
            }
        }

        [HttpPost("match")]
        public async Task<IActionResult> Match(
            [FromForm] string verificationId,
            [FromForm] IFormFile selfie,
            [FromForm] IFormFile idImage,
            [FromForm] double threshold = 0.75)
        {
            if (string.IsNullOrEmpty(verificationId) || selfie == null || idImage == null)
                return BadRequest("verificationId, selfie and id image are required");

            try
            {
                var result = await _faceService.MatchFacesAsync(verificationId, selfie, idImage, threshold);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Face Match");
                return StatusCode(500, new { error = "Face match failed" });
            }
        }

        [HttpPost("match2")]
        public async Task<IActionResult> Match2(
[FromForm] string verificationId,
[FromForm] string selfieBase64,
[FromForm] string idImageBase64,
[FromForm] double threshold = 0.75)
        {
            if (string.IsNullOrEmpty(verificationId) || string.IsNullOrEmpty(selfieBase64) || string.IsNullOrEmpty(idImageBase64))
                return BadRequest("verificationId, selfie and id image are required");


try
            {
                IFormFile selfie = ConvertBase64ToFormFile(selfieBase64, "selfie.jpg");
                IFormFile idImage = ConvertBase64ToFormFile(idImageBase64, "idImage.jpg");

                var result = await _faceService.MatchFacesAsync(verificationId, selfie, idImage, threshold);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Face Match");
                return StatusCode(500, new { error = "Face match failed" });
            }


}

        // Helper method
        private IFormFile ConvertBase64ToFormFile(string base64, string fileName)
        {
            // Remove data URL prefix if present
            var commaIndex = base64.IndexOf(',');
            if (commaIndex >= 0)
                base64 = base64.Substring(commaIndex + 1);


byte[] bytes = Convert.FromBase64String(base64);
            var stream = new MemoryStream(bytes);

            return new FormFile(stream, 0, bytes.Length, fileName, fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };


}

    }
}
