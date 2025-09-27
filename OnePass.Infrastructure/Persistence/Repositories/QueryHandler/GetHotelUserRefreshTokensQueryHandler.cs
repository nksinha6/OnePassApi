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
    public class GetHotelUserRefreshTokensQueryHandler :
    QueryHandlerBase<GetHotelUserRefreshTokensQuery, HotelUserRefreshToken>,
    IReadQueryHandler<GetHotelUserRefreshTokensQuery, HotelUserRefreshToken>
    {
        private static readonly Func<OnePassDbContext, string?, string?, int?, IAsyncEnumerable<HotelUserRefreshToken>> GetTokensQuery =
            EF.CompileAsyncQuery((OnePassDbContext ctx, string? token, string? userId, int? tenantId) =>
                from t in ctx.HotelUserRefreshToken.AsNoTracking()
                where (token == null || t.Token == token) &&
                      (userId == null || t.UserId == userId) &&
                      (tenantId == null || t.TenantId == tenantId)
                orderby t.ExpiresAt descending
                select new HotelUserRefreshToken
                {
                    Token = t.Token,
                    UserId = t.UserId,
                    TenantId = t.TenantId,
                    ExpiresAt = t.ExpiresAt,
                    IsRevoked = t.IsRevoked
                });

        public GetHotelUserRefreshTokensQueryHandler(
            OnePassDbContext context,
            Tracer tracer,
            ILogger<GetHotelUserRefreshTokensQueryHandler> logger)
            : base(context, tracer, logger)
        {
        }

        public async Task<IEnumerable<HotelUserRefreshToken>> HandleQueryAsync(GetHotelUserRefreshTokensQuery query)
        {
            return await ExecuteQuerySafelyAsync(async ctx =>
            {
                return await GetTokensQuery(ctx, query.Token, query.UserId, query.TenantId).ToListAsync();
            });
        }

        public Task<IEnumerable<HotelUserRefreshToken>> HandleAllAsync()
        {
            throw new NotSupportedException("Fetching all refresh tokens without any filter is not supported.");
        }
    }
}
