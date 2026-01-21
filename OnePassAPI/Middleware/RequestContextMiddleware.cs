using System.Security.Claims;

namespace OnePass.API
{
    public class RequestContextMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            RequestContext requestContext)
        {
            var user = context.User;

            if (user.Identity?.IsAuthenticated == true)
            {
                requestContext.UserId =
                    user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                    ?? throw new UnauthorizedAccessException("UserId missing");

                requestContext.TenantId =
                    int.Parse(user.FindFirst("tenantId")?.Value
                    ?? throw new UnauthorizedAccessException("TenantId missing"));

                requestContext.PropertyIds =
                    (user.FindFirst("propertyIds")?.Value ?? "")
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse)
                        .ToList();

                requestContext.Role =
                    user.FindFirst(ClaimTypes.Role)?.Value
                    ?? throw new UnauthorizedAccessException("Role missing");
            }

            await _next(context);
        }
    }
}
