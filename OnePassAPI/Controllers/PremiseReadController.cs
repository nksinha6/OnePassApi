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
