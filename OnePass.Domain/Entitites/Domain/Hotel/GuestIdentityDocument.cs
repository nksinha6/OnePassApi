using OnePass.Dto;

namespace OnePass.Domain
{
    public class GuestIdentityDocument
    {
        public Guid DocumentId { get; set; }
        public Guid GuestId { get; set; }
        public DocumentType DocumentType { get; set; }
        public byte[] DocumentNumber { get; set; } = null!;
        public string IssuingCountry { get; set; } = null!;
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidUntil { get; set; }
        public string? DocumentImageUrl { get; set; }
        public string? OcrExtractedName { get; set; }
        public DateTime? OcrExtractedDob { get; set; }
        public decimal? FaceMatchScore { get; set; }
        public VerificationStatus VerificationStatus { get; set; } = VerificationStatus.pending;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }

    public enum DocumentType
    {
        Aadhaar,
        Passport,
        DrivingLicense,
        VoterID,
        OCI
    }
}
