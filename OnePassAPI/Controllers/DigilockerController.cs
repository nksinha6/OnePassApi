using Microsoft.AspNetCore.Mvc;
using OnePass.Domain;

namespace DigiLockerApi
{
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
                return BadRequest("verificationId and mobileNumber required");

            var resp = await _service.VerifyAccountAsync(req.VerificationId, req.MobileNumber);
            return Ok(resp);
        }

        [HttpPost("create-url")]
        public async Task<IActionResult> CreateUrl([FromBody] CreateUrlRequest req)
        {
            var resp = await _service.CreateUrlAsync(req.VerificationId, req.DocumentRequested, req.RedirectUrl, req.UserFlow);
            return Ok(resp);
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
