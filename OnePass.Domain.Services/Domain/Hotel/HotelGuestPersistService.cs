
using System.Linq.Expressions;
using Microsoft.Extensions.Configuration;
using Npgsql;
using OnePass.Dto;

namespace OnePass.Domain.Services
{
    public class HotelGuestPersistService(IPersistRepository<HotelGuest> guestRepository,
        IPersistRepository<HotelGuestFaceCapture> hotelGuestFaceCapturePersistRepository,
        IPersistRepository<HotelGuestAadhaarImage> hotelGuestAadharImageRepository,
        IPersistRepository<HotelBookingGuest> hotelBookingGuestRepository,
        IHotelGuestReadService hotelGuestReadService,
        IStoredProcPersistRepository<DeleteGuestParam> deleteGuestRepository,
        IConfiguration configuration) : IHotelGuestPersistService
    {
        private readonly IPersistRepository<HotelGuest> _guestRepository = guestRepository;

        private readonly IHotelGuestReadService _hotelGuestReadService = hotelGuestReadService;
        private readonly IPersistRepository<HotelGuestFaceCapture> _hotelGuestFaceCapturePersistRepository = hotelGuestFaceCapturePersistRepository;

        private readonly IPersistRepository<HotelGuestAadhaarImage> _hotelGuestAadharImageRepository = hotelGuestAadharImageRepository;

        private readonly IPersistRepository<HotelBookingGuest> _hotelBookingGuestRepository = hotelBookingGuestRepository;

        private readonly IStoredProcPersistRepository<DeleteGuestParam> _deleteGuestRepository = deleteGuestRepository;
        // ✅ DRY helper method
        private static async Task<T> PersistSingleAsync<T>(IPersistRepository<T> repository, T entity) where T : class
        {
            var result = await repository.AddOrUpdateAllAsync(new List<T> { entity });
            return result.First();
        }

        public Task<HotelGuest> Persist(HotelGuest guest) =>
            PersistSingleAsync(_guestRepository, guest);

        public async Task<HotelGuest> UpdateEmailIdData(UpdateEmailIdParam param)
        {
            var guest = await _hotelGuestReadService.GetHotelGuestAsync(new GetHotelGuestByPhoneQuery()
            {
                PhoneCountryCode = param.PhoneCountryCode,
                PhoneNumber = param.PhoneNumber
            });
            if (guest.Id == Guid.Empty)
            {
                return await guestRepository.AddIfNotExistAsync(new HotelGuest()
                {
                    PhoneCountryCode = param.PhoneCountryCode,
                    PhoneNumber = param.PhoneNumber,
                    Email = param.EmailAddress,
                    VerificationStatus = VerificationStatus.pending
                });
            }

            return await _guestRepository.UpdatePartialAsync(new HotelGuest() { Id = guest.Id, 
                Email = param.EmailAddress }, x => x.Email);
        }

        public async Task<HotelGuest> UpdateAadharData(UpdateAadharStatusParam param) {

            var guest = await _hotelGuestReadService.GetHotelGuestAsync(new GetHotelGuestByPhoneQuery()
            {
                PhoneCountryCode = param.PhoneCountryCode,
                PhoneNumber = param.PhoneNumber
            });

            return await _guestRepository.UpdatePartialAsync(new HotelGuest() { Id = guest.Id, VerificationStatus = VerificationStatus.verified, FullName = param.Name, DateOfBirth = param.DateOfBirth, Gender = param.Gender, Nationality = param.Nationality, Uid =  param.Uid, VerificationId = param.VerificationId, ReferenceId = param.ReferenceId,
                
                SplitAddress = System.Text.Json.JsonSerializer.Serialize(param.SplitAddress) }, x => x.VerificationStatus, x => x.FullName, x => x.DateOfBirth, x => x.Gender, x => x.Nationality, x => x.Uid, x => x.VerificationId, x => x.ReferenceId, x => x.SplitAddress);
        }

        public Task<HotelGuestFaceCapture> PersistFaceCapture(HotelGuestFaceCapture hotelGuestFaceCapture) =>
            PersistSingleAsync(_hotelGuestFaceCapturePersistRepository, hotelGuestFaceCapture);

        public async Task<HotelGuestAadhaarImage> PersistAadharImageAsync(
    HotelGuestAadhaarImage selfie,
    Stream selfieStream)
        {
            if (selfieStream == null)
                throw new ArgumentNullException(nameof(selfieStream));

            // Convert stream → byte[]
            await using var ms = new MemoryStream();
            await selfieStream.CopyToAsync(ms);

            selfie.Image = ms.ToArray();
            selfie.FileSize = selfie.Image.LongLength;
            selfie.CreatedAt = DateTimeOffset.UtcNow;

            // Persist using repository / EF
            return await PersistSingleAsync(
                _hotelGuestAadharImageRepository,
                selfie);
        }

        public Task<HotelBookingGuest> PersistBookingGuestAsync(HotelBookingGuest hotelBookingGuest) =>
            _hotelBookingGuestRepository.AddIfNotExistAsync(hotelBookingGuest);

        public Task<bool> DeleteHotelGuest(DeleteGuestParam guest) =>
            _deleteGuestRepository.ExecuteCommandAsync(guest);
    }
}
