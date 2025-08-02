using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnePass.Domain;
using OnePass.Dto;

namespace OnePass.API.Controllers
{
    [ApiController]
    [Route("api/user/persist")]
    public class UserPersistsController(IUserPersistsService userPersistsService, ILogger<UserPersistsController> logger) : PersistBaseController
    {
        private readonly IUserPersistsService _userPersistsService = userPersistsService;
        private readonly ILogger<UserPersistsController> _logger = logger;

        [HttpPost("PersistUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> CreateProperty([FromBody] User request) =>
            ExecutePersistAsync(
                request,
                nameof(UserReadController.GetUser),
                "UserRead",
                async () =>
                {
                    return await _userPersistsService.PersistsAsync(request);
                });

        [HttpPut("UpdateUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> UpdateUser([FromBody] UpdateUserStatusParam param) =>
            ExecutePersistAsync(
                param,
                nameof(UserReadController.GetUser),
                "UserRead",
                async () =>
                {
                    return await _userPersistsService.UpdateStatusAsync(param);
                });

        [HttpPut("VerifyUserEmail")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> VerifyUserEmail([FromQuery] string phoneNo) =>
            ExecutePersistAsync(
                phoneNo,
                nameof(UserReadController.GetUser),
                "UserRead",
                async () =>
                {
                    return await _userPersistsService.VerifyEmailAsync(phoneNo);
                });

        [HttpPut("UpdateUserProfile")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> UpdateUserProfile([FromBody] UserProfileUpdateDto request) =>
            ExecutePersistAsync(
                request,
                nameof(UserReadController.GetUser),
                "UserRead",
                async () =>
                {
                    return await _userPersistsService.UpdateUserProfileAsync(request);
                });


        [HttpPost("users/{phone}/photo")]
        public async Task<IActionResult> UpdateUserPhoto(string phone, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            // ✅ Validate file type (optional)
            var allowedTypes = new[] { "image/jpeg", "image/png" };
            if (!allowedTypes.Contains(file.ContentType))
                return BadRequest("Only JPEG and PNG files are allowed.");

            // ✅ Convert file to byte[]
            byte[] photoBytes;
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                photoBytes = ms.ToArray();
            }

            return await
                ExecutePersistAsync(
                photoBytes,
                nameof(UserReadController.GetUser),
                "UserRead",
                async () =>
                {
                    return await _userPersistsService.UpdateUserImageAsync(phone, photoBytes);
                });

        }

    }
}
