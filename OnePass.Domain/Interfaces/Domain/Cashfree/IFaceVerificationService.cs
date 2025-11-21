using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace OnePass.Domain
{
    public interface IFaceVerificationService
    {
        Task<FaceLivenessResponse> CheckLivenessAsync(string verificationId, IFormFile selfie);
        Task<FaceMatchResponse> MatchFacesAsync(string verificationId, IFormFile selfie, IFormFile idImage, double threshold = 0.75);
    }
}
