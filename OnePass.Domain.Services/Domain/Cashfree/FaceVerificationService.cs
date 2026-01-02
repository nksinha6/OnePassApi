using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace OnePass.Domain.Services
{
    public class FaceVerificationService : IFaceVerificationService
    {
        private readonly HttpClient _httpClient;

        public FaceVerificationService([FromKeyedServices("Cashfree")] HttpClient httpClient)
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

        public async Task<FaceMatchResponse> MatchFacesAsync(
    string verificationId,
    IFormFile selfie,
    IFormFile idImage,
    double threshold = 0.75)
        {
            if (string.IsNullOrWhiteSpace(verificationId)) throw new ArgumentException("verificationId is required", nameof(verificationId));
            if (selfie == null) throw new ArgumentNullException(nameof(selfie));
            if (idImage == null) throw new ArgumentNullException(nameof(idImage));

            using var content = new MultipartFormDataContent();

            // scalar fields
            content.Add(new StringContent(verificationId), "verification_id"); // provider may expect snake_case
            content.Add(new StringContent(threshold.ToString(CultureInfo.InvariantCulture)), "threshold");

            // Selfie (first_image)
            var selfieStream = selfie.OpenReadStream();
            var selfieContent = new StreamContent(selfieStream);
            var selfieContentType = !string.IsNullOrWhiteSpace(selfie.ContentType) ? selfie.ContentType : "image/jpeg";
            selfieContent.Headers.ContentType = new MediaTypeHeaderValue(selfieContentType);
            // Ensure filename is provided and only the filename part (no path)
            var selfieFilename = Path.GetFileName(selfie.FileName) ?? "selfie.jpg";
            content.Add(selfieContent, "first_image", selfieFilename);

            // ID image (second_image)
            var idStream = idImage.OpenReadStream();
            var idContent = new StreamContent(idStream);
            var idContentType = !string.IsNullOrWhiteSpace(idImage.ContentType) ? idImage.ContentType : "image/jpeg";
            idContent.Headers.ContentType = new MediaTypeHeaderValue(idContentType);
            var idFilename = Path.GetFileName(idImage.FileName) ?? "id.jpg";
            content.Add(idContent, "second_image", idFilename);

            // Prepare request (use absolute URI to avoid changing HttpClient.BaseAddress)
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.cashfree.com/verification/face-match")
            {
                Content = content
            };

            using var resp = await _httpClient.SendAsync(request);
            var respBody = await resp.Content.ReadAsStringAsync();

            if (!resp.IsSuccessStatusCode)
            {
                // Log full body for debugging — Cashfree often returns a JSON error explaining what went wrong
                
                throw new HttpRequestException($"Face match failed with status {(int)resp.StatusCode}: {respBody}");
            }

            // Deserialize response (case-insensitive to be robust)
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var respObj = JsonSerializer.Deserialize<FaceMatchResponse>(respBody, options);

            return respObj ?? throw new InvalidOperationException("Empty response from face match API");
        }
    }
}
