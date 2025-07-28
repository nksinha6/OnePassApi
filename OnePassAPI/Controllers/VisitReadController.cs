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
public class VisitReadController(
    IVisitReadService visitReadService,
    ILogger<VisitReadController> logger,
    IMemoryCache cache)
    : ReadControllerBase(logger, cache)
{
    private readonly IVisitReadService _visitReadService = visitReadService;

    [HttpGet]
    public Task<ActionResult<IEnumerable<VisitPurpose>>> GetVisitPurposes() =>
        ExecuteAsync(
            null,
            () => "",
            () => _visitReadService.GetVisitPurposes(new GetAllVisitPurposesQuery()),
            notFoundMessage: $"No visit purposes found."
        );

}
