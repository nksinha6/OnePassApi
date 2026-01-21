namespace OnePass.API
{
    using System.Security.Claims;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.IdentityModel.JsonWebTokens;

    public sealed class RequestContextMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext httpContext,
            RequestContext requestContext)
        {
            // Force authentication ONLY if a token exists
            var authResult = await httpContext.AuthenticateAsync();

            if (authResult.Succeeded && authResult.Principal != null)
            {
                var user = authResult.Principal;
                httpContext.User = user;

                requestContext.UserId =
                    user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

                var tenantIdClaim = user.FindFirst("tenantId")?.Value;
                if (int.TryParse(tenantIdClaim, out var tenantId))
                {
                    requestContext.TenantId = tenantId;
                }

                requestContext.PropertyIds =
                    ParsePropertyIds(user.FindFirst("propertyIds")?.Value);

                requestContext.Role =
                    user.FindFirst(ClaimTypes.Role)?.Value;
            }

            await _next(httpContext);
        }

        private static IReadOnlyList<int> ParsePropertyIds(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return Array.Empty<int>();

            return raw.Split(',', StringSplitOptions.RemoveEmptyEntries)
                      .Select(x => int.TryParse(x, out var id) ? id : (int?)null)
                      .Where(x => x.HasValue)
                      .Select(x => x!.Value)
                      .ToList();
        }
    }

}
