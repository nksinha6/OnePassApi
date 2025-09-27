using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnePass.Domain;
using OpenTelemetry.Trace;

namespace OnePass.Infrastructure.Persistence
{
    public class GetHotelUserPasswordByIdQueryHandler :
    QueryHandlerBase<GetHotelUserPasswordQuery, HotelUserPassword>,
    IReadQueryHandler<GetHotelUserPasswordQuery, HotelUserPassword>
    {
        private static readonly Func<OnePassDbContext, string, int, IAsyncEnumerable<HotelUserPassword>> GetPasswordQuery =
            EF.CompileAsyncQuery((OnePassDbContext ctx, string userId, int tenantId) =>
                from hup in ctx.HotelUserPasswords.AsNoTracking()
                where hup.UserId == userId && hup.TenantId == tenantId
                select new HotelUserPassword
                {
                    UserId = hup.UserId,
                    TenantId = hup.TenantId,
                    PasswordHash = hup.PasswordHash
                });

        public GetHotelUserPasswordByIdQueryHandler(
            OnePassDbContext context,
            Tracer tracer,
            ILogger<GetHotelUserPasswordByIdQueryHandler> logger)
            : base(context, tracer, logger)
        {
        }

        public async Task<IEnumerable<HotelUserPassword>> HandleQueryAsync(GetHotelUserPasswordQuery query)
        {
            return await ExecuteQuerySafelyAsync(async ctx =>
            {
                return await GetPasswordQuery(ctx, query.UserId, query.TenantId).ToListAsync();
            });
        }

        public Task<IEnumerable<HotelUserPassword>> HandleAllAsync()
        {
            throw new NotSupportedException("Fetching all user passwords is not supported.");
        }
    }

}
