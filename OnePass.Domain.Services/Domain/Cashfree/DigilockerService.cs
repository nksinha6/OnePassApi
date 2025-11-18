using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;

namespace OnePass.Domain
{
    public class DigilockerService : IDigilockerService
    {
        private readonly HttpClient _httpClient;

        public DigilockerService([FromKeyedServices("Cashfree")] HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<VerifyAccountResponse> VerifyAccountAsync(string verificationId, string mobile)
        {
            _httpClient.BaseAddress = new Uri("https://sandbox.cashfree.com/verification/digilocker/");

            var req = new VerifyAccountRequest
            {
                VerificationId = Guid.NewGuid().ToString(),
                MobileNumber = mobile
            };
            var resp = await _httpClient.PostAsJsonAsync("verify-account", req);
            resp.EnsureSuccessStatusCode();
            var res = await resp.Content.ReadFromJsonAsync<VerifyAccountResponse>();

            return res;
        }

        public async Task<CreateUrlResponse> CreateUrlAsync(string verificationId, List<string> documents, string redirectUrl, string userFlow)
        {
     
            var req = new CreateUrlRequest
            {
                VerificationId = verificationId,
                DocumentRequested = documents,
                RedirectUrl = redirectUrl,
                UserFlow = userFlow
            };
            var resp = await _httpClient.PostAsJsonAsync("", req);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<CreateUrlResponse>();
        }

        public async Task<VerificationStatusResponse> GetStatusAsync(string verificationId, long? referenceId = null)
        {
            
            var url = $"?verification_id={Uri.EscapeDataString(verificationId)}";
            if (referenceId.HasValue)
                url += $"&reference_id={referenceId.Value}";
            var resp = await _httpClient.GetAsync(url);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<VerificationStatusResponse>();
        }

        public async Task<AadhaarDocumentResponse> GetAadhaarDocumentAsync(string verificationId, long referenceId)
        {
            
            var url = $"document/AADHAAR?verification_id={Uri.EscapeDataString(verificationId)}&reference_id={referenceId}";
            var resp = await _httpClient.GetAsync(url);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<AadhaarDocumentResponse>();
        }
    }
}
