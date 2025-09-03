using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class GuestFaceCapture
    {
        public Guid FaceCaptureId { get; set; }
        public Guid GuestId { get; set; }
        public Guid CheckinId { get; set; }
        public string? ImageUrl { get; set; }
        public byte[] FaceTemplate { get; set; } = null!;
        public DateTimeOffset TemplateCreatedDatetime { get; set; }
        public DateTimeOffset LiveCaptureDatetime { get; set; }
        public decimal? FaceMatchScore { get; set; }
        public FaceMatchStatus MatchStatus { get; set; } = FaceMatchStatus.NotAttempted;
        public FaceVerificationMethod VerificationMethod { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
    
    public enum FaceMatchStatus
    {
        Matched,
        Mismatch,
        ManualReview,
        NotAttempted
    }

    public enum FaceVerificationMethod
    {
        TemplateVsLive,
        TemplateVsDocImage,
        Both
    }
}
