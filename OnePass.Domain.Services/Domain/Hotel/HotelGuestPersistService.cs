
using System.Linq.Expressions;

namespace OnePass.Domain.Services
{
    public class HotelGuestPersistService(IPersistRepository<HotelGuest> guestRepository,
        IPersistRepository<HotelGuestFaceCapture> hotelGuestFaceCapturePersistRepository,
        IPersistRepository<HotelGuestSelfie> hotelGuestSelfieRepository,
        IHotelGuestReadService hotelGuestReadService) : IHotelGuestPersistService
    {
        private readonly IPersistRepository<HotelGuest> _guestRepository = guestRepository;

        private readonly IHotelGuestReadService _hotelGuestReadService = hotelGuestReadService;
        private readonly IPersistRepository<HotelGuestFaceCapture> _hotelGuestFaceCapturePersistRepository = hotelGuestFaceCapturePersistRepository;

        private readonly IPersistRepository<HotelGuestSelfie> _hotelGuestSelfieRepository = hotelGuestSelfieRepository;

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
            Guid id = guest != null ? guest.Id : Guid.NewGuid();
            if(guest == null)
            {
                var result = await guestRepository.AddIfNotExistAsync(new HotelGuest()
                {
                    PhoneCountryCode = param.PhoneCountryCode,
                    PhoneNumber = param.PhoneNumber,
                    Id = id
                });
            }

            return await _guestRepository.UpdatePartialAsync(new HotelGuest() { Id = id, VerificationStatus = VerificationStatus.Verified, FullName = param.Name}, x => x.VerificationStatus);
        }

        public Task<HotelGuestFaceCapture> PersistFaceCapture(HotelGuestFaceCapture hotelGuestFaceCapture) =>
            PersistSingleAsync(_hotelGuestFaceCapturePersistRepository, hotelGuestFaceCapture);

        public Task<HotelGuestSelfie> PersistSelfie(HotelGuestSelfie selfieRequest)
        =>
            PersistSingleAsync(_hotelGuestSelfieRepository, selfieRequest);
    }
}
