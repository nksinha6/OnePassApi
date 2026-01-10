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
    ImageInput selfie,
    ImageInput idImage,
    double threshold = 0.75,
    CancellationToken ct = default)
        {
            using var content = new MultipartFormDataContent();

            content.Add(new StringContent(verificationId), "verification_id");
            content.Add(new StringContent(
                threshold.ToString(CultureInfo.InvariantCulture)), "threshold");

            var selfieContent = new StreamContent(selfie.Stream);
            selfieContent.Headers.ContentType = new MediaTypeHeaderValue(selfie.ContentType);
            content.Add(selfieContent, "first_image", selfie.FileName);

            var idContent = new StreamContent(idImage.Stream);
            idContent.Headers.ContentType = new MediaTypeHeaderValue(idImage.ContentType);
            content.Add(idContent, "second_image", idImage.FileName);

            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://api.cashfree.com/verification/face-match")
            {
                Content = content
            };

            using var resp = await _httpClient.SendAsync(
                request,
                HttpCompletionOption.ResponseHeadersRead,
                ct
            );

            var respBody = await resp.Content.ReadAsStringAsync(ct);

            if (!resp.IsSuccessStatusCode)
                throw new HttpRequestException(
                    $"Face match failed ({(int)resp.StatusCode}): {respBody}");

            return JsonSerializer.Deserialize<FaceMatchResponse>(
                respBody,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            ) ?? throw new InvalidOperationException("Empty response");
        }
    }
}
