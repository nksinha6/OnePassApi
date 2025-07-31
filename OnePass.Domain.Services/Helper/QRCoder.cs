using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRCoder;

namespace OnePass.Domain.Services
{
    public class QRCoder
    {
        public static string GenerateQrCodeAsBase64(string data)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrData);
            byte[] qrBytes = qrCode.GetGraphic(20); // 20 = pixel per module
            return Convert.ToBase64String(qrBytes); // ✅ store as Base64 in DB
        }
    }
}
