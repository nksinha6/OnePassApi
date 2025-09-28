using System.ComponentModel.Design;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using OnePass.API;
using OnePass.Domain;
using OnePass.Dto;
using OnePass.Dto.Response;

[ApiController]
[Route("api/[controller]")]
public class CompanyReadController(
    ICompanyReadService companyReadService,
    ILogger<CompanyReadController> logger,
    IMemoryCache cache)
    : ReadControllerBase(logger, cache)
{
    private readonly ICompanyReadService _companyReadService = companyReadService;

    [HttpGet]
    [Authorize]
    public Task<ActionResult<IEnumerable<Company>>> GetCompanies() =>
        ExecuteAsync(
            null,
            () => $"",
            () => _companyReadService.GetCompaniesAsync(new GetAllCompaniesQuery()),
            notFoundMessage: $"No company found."
        );

}
