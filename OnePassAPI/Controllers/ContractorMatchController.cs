using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OnePass.Domain;
using OnePass.Domain.Services;
using OnePass.Dto;

namespace OnePass.API.Controllers
{
    [ApiController]
    [Route("api/contractor")]
    public class ContractorMatchController(IContractorReadService contractorReadService,
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
