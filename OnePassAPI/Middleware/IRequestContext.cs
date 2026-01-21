namespace OnePass.API
{
    public interface IRequestContext
    {
        string UserId { get; }
        int TenantId { get; }
        IReadOnlyList<int> PropertyIds { get; }
        string Role { get; }
    }
}
