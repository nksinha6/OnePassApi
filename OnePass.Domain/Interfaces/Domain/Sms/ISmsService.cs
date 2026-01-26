using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public interface ISmsService
    {
        Task<bool> SendOnboardingLinkSmsAsync(string phoneCountryCode, string phoneNumber, int propertyId);

        Task<bool> SendOtpSmsAsync(string to, string otp);
    }
}
