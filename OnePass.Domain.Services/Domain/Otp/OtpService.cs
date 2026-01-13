using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain.Services
{
    public class OtpService :
ReadServiceBase, IOtpService
    {
        private readonly IHasher _hasher;
        private readonly ISmsService _smsService;
        private readonly IPersistRepository<HotelGuestsOtpCode> _guestOtpRepository;

        public OtpService(
            IHasher hasher,
            ISmsService smsService,
            IPersistRepository<HotelGuestsOtpCode> guestOtpRepository,
            IReadRepositoryFactory repositoryFactory
        ) : base(repositoryFactory)  // base constructor call goes here
        {
            _hasher = hasher;
            _smsService = smsService;
            _guestOtpRepository = guestOtpRepository;
        }
        public async Task SendOtpAsync(string phoneCountryCode, string phoneNumber)
        {
            var otp = GenerateOtp(6);
            Console.WriteLine("OTP:" + otp);
            // Hash it before storing
            var hashedOtp = _hasher.Hash(otp);

            var cleanedCountryCode = phoneCountryCode.TrimStart('+');
            // Create record
            var record = new HotelGuestsOtpCode
            {
                PhoneCountryCode = cleanedCountryCode,
                PhoneNumber = phoneNumber,
                HashedOtp = hashedOtp,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(20),
                Attempts = 0,
                IsUsed = false
            };

            //if it throws
           /* await _guestOtpRepository.DeleteAsync(new HotelGuestsOtpCode()
            {
                PhoneCountryCode = phoneCountryCode,
                PhoneNumber = phoneNumber,
            });
           */
            await _guestOtpRepository.AddAsync(record);
           
            var fullPhoneNumber = cleanedCountryCode + phoneNumber;

            await _smsService.SendOtpSmsAsync(fullPhoneNumber, otp);
        }

        private static string GenerateOtp(int length)
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[length];
            rng.GetBytes(bytes);

            // Convert bytes to digits
            var digits = new char[length];
            for (int i = 0; i < length; i++)
            {
                digits[i] = (char)('0' + (bytes[i] % 10));
            }
            return new string(digits);
        }

        public async Task<HotelGuestVerifyOtpResponse> VerifyOtpAsync(string countryCode, string phoneNumber, string otp)
        {
            // Normalize inputs (optional)
            countryCode = countryCode.TrimStart('+');

            // Look up the OTP record by composite key
            var record = await HandleSingleOrDefaultAsync<GetHotelGuestOtpCodeQuery, HotelGuestsOtpCode>(
                new GetHotelGuestOtpCodeQuery()
                {
                    PhoneCountryCode = countryCode,
                    PhoneNumber = phoneNumber,
                },
                useStoredProcedure: false);

            if (record == null)
                return new HotelGuestVerifyOtpResponse
                {
                    PhoneCountryCode = countryCode,
                    PhoneNumber = phoneNumber,
                    VerificationStatus = false
                }; // no record

            // Check expiry
            if (record.ExpiresAt < DateTime.UtcNow)
                return new HotelGuestVerifyOtpResponse
                {
                    PhoneCountryCode = countryCode,
                    PhoneNumber = phoneNumber,
                    VerificationStatus = false
                }; // expired

            // Check if already used
            if (record.IsUsed)
                return new HotelGuestVerifyOtpResponse
                {
                    PhoneCountryCode = countryCode,
                    PhoneNumber = phoneNumber,
                    VerificationStatus = false
                };

            // Check attempts limit (example max 5)
            if (record.Attempts >= 5)
                return new HotelGuestVerifyOtpResponse
                {
                    PhoneCountryCode = countryCode,
                    PhoneNumber = phoneNumber,
                    VerificationStatus = false
                }; // too many attempts

            // Compare hashed OTP
            bool isValid =_hasher.Verify(otp, record.HashedOtp);

            if (!isValid)
            {
                record.Attempts++;
                await _guestOtpRepository.UpdateAsync(record);
                return new HotelGuestVerifyOtpResponse
                {
                    PhoneCountryCode = countryCode,
                    PhoneNumber = phoneNumber,
                    VerificationStatus = false
                };
            }

            // Mark OTP used
            record.IsUsed = true;
            await _guestOtpRepository.UpdateAsync(record);
            return new HotelGuestVerifyOtpResponse
            {
                PhoneCountryCode = countryCode,
                PhoneNumber = phoneNumber,
                VerificationStatus = true
            };
        }

    }
}
