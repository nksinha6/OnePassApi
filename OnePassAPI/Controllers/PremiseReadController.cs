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

    [HttpGet("property-by-companyId")]
    public Task<ActionResult<PropertyResponseContainer>> GetPropertyByCompanyId([FromQuery] Guid companyId) =>
    ExecuteAsync(
        companyId,
        () => $"premises_parent_{companyId}",
        async () =>
        {
            var properties = await _premiseReadService.GetPropertyAsync(
                new GetPropertiesByCompanyIdQuery { CompanyId = companyId }
            );
            return properties.Adapt<PropertyResponseContainer>();
        },
        notFoundMessage: $"No property found for CompanyId {companyId}."
    );


    [HttpGet("units-by-companyId")]
    public Task<ActionResult<UnitResponseContainer>> GetUnitsByCompanyId([FromQuery] Guid companyId) =>
        ExecuteAsync(
            companyId,
            () => $"premises_parent_{companyId}",
             async () =>
             {
                 var units = await _premiseReadService.GetUnitsAsync(new GetUnitsByCompanyIdQuery { CompanyId = companyId }
                 );
                 return units.Adapt<UnitResponseContainer>();
             },
            notFoundMessage: $"No unit found for CompanyId {companyId}."
        );

    [HttpGet("units-by-propertyId")]
    public Task<ActionResult<UnitResponseContainer>>GetUnitsByPropertyId([FromQuery] Guid propertyId) =>
        ExecuteAsync(
            propertyId,
            () => $"premises_parent_{propertyId}",
        async () =>
        {
            var units = await _premiseReadService.GetUnitsAsync(new GetUnitsByPropertyIdQuery { PropertyId = propertyId }
            );
            return units.Adapt<UnitResponseContainer>();
        },
            notFoundMessage: $"No unit found for PropertyId {propertyId}."
        );

    [HttpGet("desks-by-companyId")]
    public Task<ActionResult<DeskResponseContainer>> GetDesksByCompanyId([FromQuery] Guid companyId) =>
        ExecuteAsync(
            companyId,
            () => $"premises_parent_{companyId}",
    async () =>
    {
                var units = await _premiseReadService.GetDesksAsync(new GetDesksByCompanyIdQuery { CompanyId = companyId }
                );
                return units.Adapt<DeskResponseContainer>();
            },
            notFoundMessage: $"No Desk found for CompanyId {companyId}."
        );

    [HttpGet("desks-by-unitId")]
    public Task<ActionResult<DeskResponseContainer>> GetDesksByUnitId([FromQuery] Guid unitId) =>
        ExecuteAsync(
            unitId,
            () => $"premises_parent_{unitId}",
    async () =>
    {
                var units = await _premiseReadService.GetDesksAsync(new GetDesksByUnitIdQuery { UnitId = unitId }
                );
                return units.Adapt<DeskResponseContainer>();
            },
            notFoundMessage: $"No desk found for UnitId {unitId}."
        );
}
