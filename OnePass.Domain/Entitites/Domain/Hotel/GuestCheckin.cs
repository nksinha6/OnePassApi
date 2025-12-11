namespace OnePass.Infrastructure.Persistence
{
    public class GuestCheckin
    {
        public Guid CheckinId { get; set; }
        public int PropertyId { get; set; }
        public Guid PrimaryGuestId { get; set; }
        public DateTimeOffset CheckinDatetime { get; set; }
        public DateTimeOffset? CheckoutDatetime { get; set; }
        public VerificationMethod VerificationMethod { get; set; }
        public CheckinVerificationStatus VerificationStatus { get; set; } = CheckinVerificationStatus.Yellow;
        public string? RoomNumber { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }

    public enum VerificationMethod
    {
        AadhaarFace,
        PassportFace,
        Manual
    }

    public enum CheckinVerificationStatus
    {
        Green,
        Yellow,
        Red
    }
}
