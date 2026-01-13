using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public interface IOtpService
    {
        Task SendOtpAsync(string countryCode, string phoneNumber);

        Task<HotelGuestVerifyOtpResponse> VerifyOtpAsync(string countryCode, string phoneNumber, string otp);
    }

}
