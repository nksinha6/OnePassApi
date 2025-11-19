using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OnePass.Domain;

namespace DigiLockerApi
{
    [EnableCors("AllowAll")]
    [ApiController]
    [Route("api/digilocker")]
    public class DigilockerController : ControllerBase
    {
        private readonly IDigilockerService _service;

        public DigilockerController(IDigilockerService service)
        {
            _service = service;
        }

        [HttpPost("verify-account")]
        public async Task<IActionResult> Verify([FromBody] VerifyAccountRequest req)
        {
            if (req == null || string.IsNullOrEmpty(req.VerificationId) || string.IsNullOrEmpty(req.MobileNumber))
            {
                return BadRequest("verificationId and mobileNumber required");
            }

            try
            {
                var resp = await _service.VerifyAccountAsync(req.VerificationId, req.MobileNumber);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in VerifyAccount: "+ex.Message);

                // Return a 500 but CORS middleware will apply
                return StatusCode(500, new { error = "Internal server error" });
            }
        }


        [HttpPost("create-url")]
        public async Task<IActionResult> CreateUrl([FromBody] CreateUrlRequest req)
        {
            if (req == null || string.IsNullOrEmpty(req.VerificationId) || req.DocumentRequested == null)
            {
                return BadRequest("verificationId, documentRequested and redirectUrl required");
            }

            try
            {
                var resp = await _service.CreateUrlAsync(
                    req.VerificationId,
                    req.DocumentRequested,
                    req.RedirectUrl,
                    req.UserFlow
                );
                return Ok(resp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in CreateUrl: "+ex.Message);
                return StatusCode(500, new { error = "Internal server error" });
            }
        }


        [HttpGet("status")]
        public async Task<IActionResult> Status([FromQuery] string verificationId, [FromQuery] long? referenceId)
        {
            var resp = await _service.GetStatusAsync(verificationId, referenceId);
            return Ok(resp);
        }

        [HttpGet("aadhaar")]
        public async Task<IActionResult> GetAadhaar([FromQuery] string verificationId, [FromQuery] long referenceId)
        {
            var resp = await _service.GetAadhaarDocumentAsync(verificationId, referenceId);
            return Ok(resp);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook([FromBody] object payload, [FromHeader(Name = "x-webhook-signature")] string signature)
        {
            // TODO: Validate signature
            // TODO: Parse payload for verification_id / reference_id, status
            // TODO: Update your DB accordingly
            Console.WriteLine(payload);
            return Ok();
        }
    }
}
