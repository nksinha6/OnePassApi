using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OnePass.Domain.Services
{
    public class AzapiService : IAzapiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "prod-72a676bb8f350498a7db8b1ec54ab3bea85997d8067070fb90bd5a510f0259d6";
        private readonly string _endpoint;

        public AzapiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _endpoint = "https://api.azapi.ai/kyc/overseas";
        }

        public async Task<PassportData> ExtractPassportAsync(byte[] imageBytes)
        {
            var endpoint = "https://ocr.azapi.ai/ind0005d"; // ✅ correct endpoint
            var apiKey = "prod-72a676bb8f350498a7db8b1ec54ab3bea85997d8067070fb90bd5a510f0259d6";

            var form = new MultipartFormDataContent();

            // FRONT image (required)
            var frontContent = new ByteArrayContent(imageBytes);
            frontContent.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");

            form.Add(frontContent, "front", "passport.jpg");

            // BACK image (optional)
            // If you have second image, add it like this:
            // form.Add(backContent, "back", "back.jpg");

            var request = new HttpRequestMessage(HttpMethod.Post, endpoint);

            // ✅ IMPORTANT: no Bearer
            request.Headers.Add("Authorization", apiKey);

            request.Content = form;

            var response = await _httpClient.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"AZAPI Error: {response.StatusCode} - {json}");
            }

            return ParsePassportResponse(json);
        }

        private PassportData ParsePassportResponse(string json)
        {
            dynamic obj = JsonConvert.DeserializeObject(json);

            var output = obj?.output;
          
            return new PassportData
            {
                full_name = output?.id_name,
                passport_number = output?.id_number,
                dob = output?.id_dob,
                expiry_date = output?.id_doe,
                nationality = output?.id_nationality,
                mrz = null, // not provided directly
                face_image = null, // not provided in this API
                address = output?.id_address
            };
        }

        public async Task<PassportData> ExtractPassportFromBase64Async(string base64Image)
        {
            if (string.IsNullOrWhiteSpace(base64Image))
                throw new ArgumentException("Base64 image is empty");

            var requestBody = new
            {
                document_type = "passport",
                image = base64Image
            };

            var request = new HttpRequestMessage(HttpMethod.Post, _endpoint);

            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);

            request.Content = new StringContent(
                JsonConvert.SerializeObject(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            try
            {
                var response = await _httpClient.SendAsync(request);

                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"AZAPI Error: {response.StatusCode} - {json}");
                }

                var result = JsonConvert.DeserializeObject<AzapiResponse>(json);

                if (result == null || result.data == null)
                    throw new Exception("Invalid response from AZAPI");

                return result.data;
            }
            catch (TaskCanceledException)
            {
                throw new Exception("AZAPI request timed out");
            }
            catch (Exception ex)
            {
                throw new Exception($"AZAPI processing failed: {ex.Message}", ex);
            }
        }
    }
}
