namespace OnePass.API
{
    public sealed class RequestContext : IRequestContext
    {
        public string? UserId { get; set; }
        public int? TenantId { get; set; }
        public IReadOnlyList<int> PropertyIds { get; set; } = [];
        public string? Role { get; set; }

        public bool IsAuthenticated => TenantId.HasValue;
    }

}
