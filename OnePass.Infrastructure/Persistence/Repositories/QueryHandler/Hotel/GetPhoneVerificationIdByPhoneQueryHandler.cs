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
    public class GetPhoneVerificationIdByPhoneQueryHandler :
    QueryHandlerBase<GetPhoneVerificationIdByPhoneQuery, PhoneVerificationId>,
    IReadQueryHandler<GetPhoneVerificationIdByPhoneQuery, PhoneVerificationId>
    {
        private static readonly Func<OnePassDbContext, string, string, Task<PhoneVerificationId>>
            GetLatestPhoneVerificationCompiledQuery =
                EF.CompileAsyncQuery((OnePassDbContext ctx, string phoneCountryCode, string phoneNumber) =>
                    ctx.PhoneVerificationIds
                       .AsNoTracking()
                       .Where(x => x.PhoneCountryCode == phoneCountryCode
                                   && x.PhoneNumber == phoneNumber)
                       .OrderByDescending(x => x.CreatedAt)
                       .FirstOrDefault());

        public GetPhoneVerificationIdByPhoneQueryHandler(
            OnePassDbContext context,
            Tracer tracer,
            ILogger<GetPhoneVerificationIdByPhoneQueryHandler> logger)
            : base(context, tracer, logger)
        {
        }

        public async Task<IEnumerable<PhoneVerificationId>> HandleAllAsync()
        {
            throw new NotSupportedException("Fetching all phone verification ids is not supported in this query.");
        }

        public async Task<IEnumerable<PhoneVerificationId>> HandleQueryAsync(GetPhoneVerificationIdByPhoneQuery query)
        {
            return await ExecuteQuerySafelyAsync(async ctx =>
            {
                var result = await GetLatestPhoneVerificationCompiledQuery(
                    ctx,
                    query.PhoneCountryCode,
                    query.PhoneNumber);

                return result != null
                    ? new List<PhoneVerificationId> { result }
                    : Enumerable.Empty<PhoneVerificationId>();
            });
        }
    }
}
