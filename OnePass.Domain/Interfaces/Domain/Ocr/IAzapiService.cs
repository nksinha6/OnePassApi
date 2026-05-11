using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public interface IAzapiService
    {
        /// <summary>
        /// Extract passport data from image bytes using AZAPI OCR
        /// </summary>
        /// <param name="imageBytes">Raw image file bytes</param>
        /// <returns>Structured passport data</returns>
        Task<PassportData> ExtractPassportAsync(byte[] imageBytes);

        /// <summary>
        /// Extract passport data from base64 string (optional overload)
        /// </summary>
        /// <param name="base64Image">Base64 encoded image</param>
        /// <returns>Structured passport data</returns>
        Task<PassportData> ExtractPassportFromBase64Async(string base64Image);
    }
}
