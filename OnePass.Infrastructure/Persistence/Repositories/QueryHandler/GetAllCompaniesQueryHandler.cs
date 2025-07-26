using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnePass.Domain;
using OpenTelemetry.Trace;

namespace OnePass.Infrastructure.Persistence
{
    public class GetAllCompaniesQueryHandler :
    QueryHandlerBase<GetAllCompaniesQuery, Company>,
    IReadQueryHandler<GetAllCompaniesQuery, Company>
    {
        private static readonly Func<OnePassDbContext, IAsyncEnumerable<Company>> GetAllCompaniesCompiledQuery =
            EF.CompileAsyncQuery((OnePassDbContext ctx) =>
                from c in ctx.Companies.AsNoTracking()
                orderby c.Name
                select new Company
                {
                    Id = c.Id,
                    Name = c.Name,
                    Address = c.Address,
                    State = c.State,
                    Zip = c.Zip,
                    City = c.City,
                    Country = c.Country,
                    Website = c.Website,
                    Email = c.Email,
                    Phone = c.Phone,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                });

        public GetAllCompaniesQueryHandler(
            OnePassDbContext context,
            Tracer tracer,
            ILogger<GetAllCompaniesQueryHandler> logger)
            : base(context, tracer, logger)
        {
        }

        public async Task<IEnumerable<Company>> HandleQueryAsync(GetAllCompaniesQuery query)
        {
            return await ExecuteQuerySafelyAsync(async ctx =>
            {
                return await GetAllCompaniesCompiledQuery(ctx).ToListAsync();
            });
        }

        public Task<IEnumerable<Company>> HandleAllAsync()
        {
            throw new NotSupportedException("HandleAllAsync is not supported for GetAllCompaniesQuery.");
        }
    }

}
