using Microsoft.AspNetCore.Mvc;
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
    }
}
