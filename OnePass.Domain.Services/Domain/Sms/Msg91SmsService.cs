using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace OnePass.Domain.Services
{
    public class Msg91Options
    {
        public string AuthKey { get; set; }
        public string SenderId { get; set; }
        public string Route { get; set; }
        public string ApiUrl { get; set; }
        public string TemplateId { get; set; }
    }


    public class Msg91SmsService : ISmsService
    {
        private readonly HttpClient _http;
        private readonly Msg91Options _opts;
        private readonly ILogger<Msg91SmsService> _logger;


        public Msg91SmsService(HttpClient http, Microsoft.Extensions.Options.IOptions<Msg91Options> opts, ILogger<Msg91SmsService> logger)
        {
            _http = http ?? throw new ArgumentNullException(nameof(http));
            _opts = opts?.Value ?? throw new ArgumentNullException(nameof(opts));
            _logger = logger;
        }

        public async Task<bool> SendOnboardingLinkSmsAsync(string to)
        {
            
            // ✅ Normalize phone
            var recipient = System.Text.RegularExpressions.Regex
                .Replace(to ?? string.Empty, "\\D", "");

            // ✅ Convert route
            int routeInt = int.Parse(_opts.Route);
            var variables = new Dictionary<string, string>();
            variables["var1"] = "https://seashell-app-dmof6.ondigitalocean.app/";

            // ✅ FINAL DLT TEMPLATE PAYLOAD (EXACT MATCH)
            var payload = new
            {
                route = routeInt,
                sender = _opts.SenderId,
                unicode = 0,
                mobiles = recipient,
                short_url = 1,
                templateId = _opts.TemplateId,
                variables = variables
            };

            var json = System.Text.Json.JsonSerializer.Serialize(payload);

            using var httpReq = new HttpRequestMessage(HttpMethod.Post, _opts.ApiUrl);
            httpReq.Headers.Add("authkey", _opts.AuthKey);
            httpReq.Headers.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            httpReq.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            try
            {
                var resp = await _http.SendAsync(httpReq);
                var body = await resp.Content.ReadAsStringAsync();

                if (!resp.IsSuccessStatusCode)
                {
                    _logger.LogWarning("MSG91 ERROR: {Status} - {Body}", resp.StatusCode, body);
                    return false;
                }

                _logger.LogInformation("✅ MSG91 SUCCESS for {Recipient}: {Body}", recipient, body);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ MSG91 EXCEPTION for {Recipient}", recipient);
                return false;
            }
        }

        public async Task<bool> SendOtpSmsAsync(string to, string otp)
        {
            var recipient = System.Text.RegularExpressions.Regex
                .Replace(to ?? string.Empty, "\\D", "");

            // ✅ Convert route
            int routeInt = int.Parse(_opts.Route);
            var variables = new Dictionary<string, string>();
            variables["var1"] = otp;

            // ✅ FINAL DLT TEMPLATE PAYLOAD (EXACT MATCH)
            var payload = new
            {
                route = routeInt,
                sender = _opts.SenderId,
                unicode = 0,
                mobiles = recipient,
                short_url = 0,
                templateId = _opts.TemplateId,
                variables = variables
            };

            var json = System.Text.Json.JsonSerializer.Serialize(payload);

            using var httpReq = new HttpRequestMessage(HttpMethod.Post, _opts.ApiUrl);
            httpReq.Headers.Add("authkey", _opts.AuthKey);
            httpReq.Headers.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            httpReq.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            try
            {
                var resp = await _http.SendAsync(httpReq);
                var body = await resp.Content.ReadAsStringAsync();

                if (!resp.IsSuccessStatusCode)
                {
                    _logger.LogWarning("MSG91 ERROR: {Status} - {Body}", resp.StatusCode, body);
                    return false;
                }

                _logger.LogInformation("✅ MSG91 SUCCESS for {Recipient}: {Body}", recipient, body);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ MSG91 EXCEPTION for {Recipient}", recipient);
                return false;
            }
        }
    }
}
