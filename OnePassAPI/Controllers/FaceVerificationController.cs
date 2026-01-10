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
                var selfieInput = new ImageInput(
    selfie.OpenReadStream(),
    Path.GetFileName(selfie.FileName),
    selfie.ContentType ?? "image/jpeg",
    selfie.Length
);

                var idInput = new ImageInput(
                    idImage.OpenReadStream(),
                    Path.GetFileName(idImage.FileName),
                    idImage.ContentType ?? "image/jpeg",
                    idImage.Length
                );

                var res = await _faceService.MatchFacesAsync(
                    verificationId,
                    selfieInput,
                    idInput
                );
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Face Match");
                return StatusCode(500, new { error = "Face match failed" });
            }
        }

    }
}

        
