using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace OnePass.Domain.Services
{
    public class FaceVerificationService : IFaceVerificationService
    {
        private readonly HttpClient _httpClient;

        public FaceVerificationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<FaceLivenessResponse> CheckLivenessAsync(string verificationId, IFormFile selfie)
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(verificationId), "verification_id");

            // Read selfie into a stream
            using var selfieStream = selfie.OpenReadStream();
            var selfieContent = new StreamContent(selfieStream);
            selfieContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(selfie.ContentType);
            content.Add(selfieContent, "image", selfie.FileName);

            var resp = await _httpClient.PostAsync("face-liveness", content);
            resp.EnsureSuccessStatusCode();
            var respObj = await resp.Content.ReadFromJsonAsync<FaceLivenessResponse>();
            return respObj;
        }

        public async Task<FaceMatchResponse> MatchFacesAsync(string verificationId, IFormFile selfie, IFormFile idImage, double threshold = 0.75)
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(verificationId), "verification_id");
            content.Add(new StringContent(threshold.ToString(System.Globalization.CultureInfo.InvariantCulture)), "threshold");

            // Selfie
            using var selfieStream = selfie.OpenReadStream();
            var selfieContent = new StreamContent(selfieStream);
            selfieContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(selfie.ContentType);
            content.Add(selfieContent, "first_image", selfie.FileName);

            // ID image (Aadhaar)
            using var idStream = idImage.OpenReadStream();
            var idContent = new StreamContent(idStream);
            idContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(idImage.ContentType);
            content.Add(idContent, "second_image", idImage.FileName);

            var resp = await _httpClient.PostAsync("face-match", content);
            resp.EnsureSuccessStatusCode();
            var respObj = await resp.Content.ReadFromJsonAsync<FaceMatchResponse>();
            return respObj;
        }
    }
}
