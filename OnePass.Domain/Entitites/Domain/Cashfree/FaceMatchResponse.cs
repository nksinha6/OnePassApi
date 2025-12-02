using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class FaceLivenessResponse
    {
        public long ReferenceId { get; set; }
        public string VerificationId { get; set; }
        public string Status { get; set; }
        public bool Liveness { get; set; }
        public double LivenessScore { get; set; }
        public double EyeWearConfidence { get; set; }
        // ... you can add other fields (gender, age_range, etc) based on docs :contentReference[oaicite:0]{index=0}
    }

    public class FaceMatchResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("ref_id")]
        public long RefId { get; set; }

        [JsonPropertyName("verification_id")]
        public string VerificationId { get; set; }

        [JsonPropertyName("face_match_result")]
        public string FaceMatchResult { get; set; }

        [JsonPropertyName("face_match_score")]
        public double FaceMatchScore { get; set; }
    }

}
