using Microsoft.AspNetCore.Mvc;
using OnePass.Domain;

namespace OnePass.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SmsController : ControllerBase
    {
        private readonly ISmsService _smsService;
        private readonly ILogger<SmsController> _logger;


        public SmsController(ISmsService smsService, ILogger<SmsController> logger)
        {
            _smsService = smsService;
            _logger = logger;
        }


        /// <summary>
        /// Sends an SMS to a single recipient via MSG91
        /// POST /api/sms/send
        /// Body: { "to": "+9199xxxxxx", "message": "Hello! Visit https://..." }
        /// </summary>
        [HttpPost("send")]
        public async Task<IActionResult> Send([FromQuery] string phoneCountryCode, [FromQuery] string phoneNumber)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);


            try
            {
                var ok = await _smsService.SendOnboardingLinkSmsAsync(phoneCountryCode, phoneNumber, 1);
                if (!ok) return StatusCode(502, new { error = "MSG91 returned an error" });


                return Ok(new { success = true });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error sending SMS");
                return StatusCode(500, new { error = "internal error" });
            }
        }
    }
}

