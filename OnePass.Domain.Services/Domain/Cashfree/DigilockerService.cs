using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using OnePass.Domain.Services;

namespace OnePass.Domain
{
    public class DigilockerService : IDigilockerService
    {
        private readonly HttpClient _httpClient;
        private readonly IHotelGuestPersistService _hotelGuestPersistService;

        public DigilockerService([FromKeyedServices("Cashfree")] HttpClient httpClient, IHotelGuestPersistService hotelGuestPersistService)
        {
            _httpClient = httpClient;
            _hotelGuestPersistService = hotelGuestPersistService;
        }

        public async Task<VerifyAccountResponse> VerifyAccountAsync(string verificationId, string mobile)
        {
            _httpClient.BaseAddress = new Uri("https://api.cashfree.com/verification/digilocker/");
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
            _httpClient.BaseAddress = new Uri("https://api.cashfree.com/verification/digilocker");
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

            _httpClient.BaseAddress = new Uri("https://api.cashfree.com/verification/digilocker");
            var url = $"?verification_id={Uri.EscapeDataString(verificationId)}";
            if (referenceId.HasValue)
                url += $"&reference_id={referenceId.Value}";
            var resp = await _httpClient.GetAsync(url);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<VerificationStatusResponse>();
        }

        public async Task<AadhaarDocumentResponse> GetAadhaarDocumentAsync(AadhaarFetchRequest request)
        {
            _httpClient.BaseAddress = new Uri("https://api.cashfree.com/verification/digilocker/");
            var url = $"document/AADHAAR?verification_id={Uri.EscapeDataString(request.VerificationId)}&reference_id={request.ReferenceId}";
            var resp = await _httpClient.GetAsync(url);
            resp.EnsureSuccessStatusCode();
            var result = await resp.Content.ReadFromJsonAsync<AadhaarDocumentResponse>();
            var param = new UpdateAadharStatusParam()
            {
                PhoneCountryCode = request.PhoneCountryCode,
                PhoneNumber = request.PhoneNumber,
                Name = result.Name
            };

            await _hotelGuestPersistService.UpdateAadharData(param);
            return result;
        }
    }
}
