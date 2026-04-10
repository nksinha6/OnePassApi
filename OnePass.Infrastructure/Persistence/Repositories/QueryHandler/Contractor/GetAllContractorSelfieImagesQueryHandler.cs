using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Infrastructure.Persistence
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using OnePass.Domain;
    using OpenTelemetry.Trace;

    public class GetAllContractorSelfieImagesQueryHandler :
    QueryHandlerBase<GetAllContractorSelfieImagesQuery, ContractorSelfieImageResponse>,
    IReadQueryHandler<GetAllContractorSelfieImagesQuery, ContractorSelfieImageResponse>
    {
        private static readonly Func<
            OnePassDbContext,
            IAsyncEnumerable<ContractorSelfieImageResponse>
        > GetAllContractorSelfieImagesCompiledQuery =
            EF.CompileAsyncQuery(
                (OnePassDbContext ctx) =>

                    ctx.ContractorSelfieImages
                       .AsNoTracking()
                       .Select(p => new ContractorSelfieImageResponse
                       {
                           PhoneCountryCode = p.PhoneCountryCode,
                           PhoneNumber = p.PhoneNumber,

                           Image = p.Image,
                           ContentType = p.ContentType,
                           FileSize = p.FileSize
                       })
            );

        public GetAllContractorSelfieImagesQueryHandler(
            OnePassDbContext context,
            Tracer tracer,
            ILogger<GetAllContractorSelfieImagesQueryHandler> logger)
            : base(context, tracer, logger)
        {
        }

        public async Task<IEnumerable<ContractorSelfieImageResponse>> HandleAllAsync()
        {
            return await ExecuteQuerySafelyAsync(async ctx =>
                await GetAllContractorSelfieImagesCompiledQuery(ctx).ToListAsync());
        }

        public async Task<IEnumerable<ContractorSelfieImageResponse>> HandleQueryAsync(
            GetAllContractorSelfieImagesQuery query)
        {
            return await HandleAllAsync();
        }
    }
}
