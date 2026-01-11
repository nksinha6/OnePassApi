
using System.Linq.Expressions;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace OnePass.Domain.Services
{
    public class HotelGuestPersistService(IPersistRepository<HotelGuest> guestRepository,
        IPersistRepository<HotelGuestFaceCapture> hotelGuestFaceCapturePersistRepository,
        IPersistRepository<HotelGuestSelfie> hotelGuestSelfieRepository,
        IHotelGuestReadService hotelGuestReadService,
        IConfiguration configuration) : IHotelGuestPersistService
    {
        private readonly IPersistRepository<HotelGuest> _guestRepository = guestRepository;

        private readonly IHotelGuestReadService _hotelGuestReadService = hotelGuestReadService;
        private readonly IPersistRepository<HotelGuestFaceCapture> _hotelGuestFaceCapturePersistRepository = hotelGuestFaceCapturePersistRepository;

        private readonly IPersistRepository<HotelGuestSelfie> _hotelGuestSelfieRepository = hotelGuestSelfieRepository;

        private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection");

        // ✅ DRY helper method
        private static async Task<T> PersistSingleAsync<T>(IPersistRepository<T> repository, T entity) where T : class
        {
            var result = await repository.AddOrUpdateAllAsync(new List<T> { entity });
            return result.First();
        }

        public Task<HotelGuest> Persist(HotelGuest guest) =>
            PersistSingleAsync(_guestRepository, guest);

        public async Task<HotelGuest> UpdateAadharData(UpdateAadharStatusParam param) {
            var guest = await _hotelGuestReadService.GetHotelGuestAsync(new GetHotelGuestByPhoneQuery()
            {
                PhoneCountryCode = param.PhoneCountryCode,
                PhoneNumber = param.PhoneNumber
            });
            if(guest.Id == Guid.Empty)
            {
                return await guestRepository.AddIfNotExistAsync(new HotelGuest()
                {
                    PhoneCountryCode = param.PhoneCountryCode,
                    PhoneNumber = param.PhoneNumber,
                    VerificationStatus = VerificationStatus.Verified
                });
            }

            return await _guestRepository.UpdatePartialAsync(new HotelGuest() { Id = guest.Id, VerificationStatus = VerificationStatus.Verified, FullName = param.Name}, x => x.VerificationStatus, x => x.FullName);
        }

        public Task<HotelGuestFaceCapture> PersistFaceCapture(HotelGuestFaceCapture hotelGuestFaceCapture) =>
            PersistSingleAsync(_hotelGuestFaceCapturePersistRepository, hotelGuestFaceCapture);

        public async Task<HotelGuestSelfie> PersistSelfieAsync(
    HotelGuestSelfie selfie,
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
                _hotelGuestSelfieRepository,
                selfie);
        }
            
    }
}
