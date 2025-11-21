using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public string Status { get; set; }
        public long RefId { get; set; }
        public string VerificationId { get; set; }
        public string FaceMatchResult { get; set; }
        public double FaceMatchScore { get; set; }
    }

}
