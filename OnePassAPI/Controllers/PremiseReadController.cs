using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OnePass.API;
using OnePass.Domain;

[ApiController]
[Route("api/[controller]")]
public class PremiseReadController(
    IPremiseReadService premiseReadService,
    ILogger<PremiseReadController> logger,
    IMemoryCache cache)
    : ReadControllerBase(logger, cache)
{
    private readonly IPremiseReadService _premiseReadService = premiseReadService;

    [HttpGet]
    public Task<ActionResult<PremiseResponse>> GetPremiseById([FromQuery] Guid id) =>
        ExecuteAsync(
            id,
            () => $"premise_{id}",
            () => _premiseReadService.GetPremiseAsync(new GetPremiseByIdQuery { Id = id }),
            notFoundMessage: $"Premise with Id {id} not found."
        );

    [HttpGet("by-tenant")]
    public Task<ActionResult<IEnumerable<PremiseResponse>>> GetPremisesByTenantId([FromQuery] Guid tenantId) =>
        ExecuteAsync(
            tenantId,
            () => $"premises_tenant_{tenantId}",
            () => _premiseReadService.GetPremisesAsync(new GetPremisesByTenantIdQuery { TenantId = tenantId }),
            notFoundMessage: $"No premises found for TenantId {tenantId}."
        );

    [HttpGet("by-parent")]
    public Task<ActionResult<IEnumerable<PremiseResponse>>> GetPremisesByParentId([FromQuery] Guid parentId) =>
        ExecuteAsync(
            parentId,
            () => $"premises_parent_{parentId}",
            () => _premiseReadService.GetPremisesAsync(new GetPremisesByParentIdQuery { ParentId = parentId }),
            notFoundMessage: $"No premises found for ParentId {parentId}."
        );
}
