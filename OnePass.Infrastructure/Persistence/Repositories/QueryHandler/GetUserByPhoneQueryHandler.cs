using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnePass.Domain;
using OpenTelemetry.Trace;

namespace OnePass.Infrastructure.Persistence
{
    public class GetUserByPhoneQueryHandler :
        QueryHandlerBase<GetUserQuery, User>,
        IReadQueryHandler<GetUserQuery, User>
    {
        // ✅ Compiled Query for performance
        private static readonly Func<OnePassDbContext, string, Task<User?>> GetUserByPhoneCompiledQuery =
            EF.CompileAsyncQuery((OnePassDbContext ctx, string phone) =>
                ctx.Users.AsNoTracking()
                    .Where(u => u.Phone == phone)
                    .Select(u => new User
                    {
                        Phone = u.Phone,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Email = u.Email,
                        Status = u.Status,
                        IsEmailVerified = u.IsEmailVerified,
                        CreatedAt = u.CreatedAt
                    })
                    .FirstOrDefault());

        public GetUserByPhoneQueryHandler(
            OnePassDbContext context,
            Tracer tracer,
            ILogger<GetUserByPhoneQueryHandler> logger)
            : base(context, tracer, logger)
        {
        }

        public async Task<IEnumerable<User>> HandleQueryAsync(GetUserQuery query)
        {
            return await ExecuteQuerySafelyAsync(async ctx =>
            {
                var user = await GetUserByPhoneCompiledQuery(ctx, query.Phone);
                return user != null ? new List<User> { user } : Enumerable.Empty<User>();
            });
        }

        public Task<IEnumerable<User>> HandleAllAsync()
        {
            throw new NotSupportedException("HandleAllAsync is not supported for GetUserByPhoneQuery.");
        }
    }
}
