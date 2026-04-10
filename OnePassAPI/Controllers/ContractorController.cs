using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OnePass.Domain;
using OnePass.Domain.Services;
using OnePass.Dto;
using static QRCoder.PayloadGenerator;

namespace OnePass.API.Controllers
{
    [ApiController]
    [Route("api/contractor")]
    public class ContractorController(IContractorReadService contractorReadService,
IRequestContext context,
ILogger<HotelGuestReadController> logger,
    IMemoryCache cache)
    : ReadControllerBase(logger, cache)
    {
        private readonly IContractorReadService _contractorReadService = contractorReadService;

        [HttpPost("match")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> MatchBookingGuestSelfie([FromForm] ContractorInputSelfieRequest request)
        {
            
            var verificationResult = await _contractorReadService.CompareSelfie(await ToImageInputAsync(request));

            return Ok(new
            {
                Result = new
                {
                    FaceMatchResult = verificationResult.Item1,
                    PhoneCountryCode = verificationResult.Item2,
                    PhoneNumber = verificationResult.Item3
                }
            });
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddContractor([FromBody] ContractorPhoneDto request)
        {
            bool status = await _contractorReadService.AddContractor(new ContractorPhoneParam()
            {
                p_phone_country_code = request.PhoneCountryCode,
                p_phone_number = request.PhoneNumber,
            });

            return status ? Ok("Successfully added") : NotFound("Guest Not Found");

        }

        public static async Task<ImageInput> ToImageInputAsync(ContractorInputSelfieRequest request)
        {
            var file = request.Selfie;

            var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            return new ImageInput(
                Stream: stream,
                FileName: file.FileName,
                ContentType: file.ContentType,
                Length: file.Length
            );
        }
    }
}
