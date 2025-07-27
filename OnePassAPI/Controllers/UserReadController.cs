using System.ComponentModel.Design;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using OnePass.API;
using OnePass.Domain;
using OnePass.Dto;
using OnePass.Dto.Response;

[ApiController]
[Route("api/[controller]")]
public class UserReadController(
    IUserReadService userReadService,
    ILogger<UserReadController> logger,
    IMemoryCache cache)
    : ReadControllerBase(logger, cache)
{
    private readonly IUserReadService _userReadService = userReadService;

    [HttpGet]
    public Task<ActionResult<User>> GetUser([FromQuery] string Id) =>
        ExecuteAsync(
            null,
            () => $"{Id}",
            () => _userReadService.GetUserAsync(new GetUserQuery() { Phone = Id}),
            notFoundMessage: $"No company found."
        );

}
