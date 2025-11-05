
using System.Linq.Expressions;

namespace OnePass.Domain.Services
{
    public class HotelGuestPersistService(IPersistRepository<HotelGuest> guestRepository,
        IPersistRepository<HotelGuestFaceCapture> hotelGuestFaceCapturePersistRepository) : IHotelGuestPersistService
    {
        private readonly IPersistRepository<HotelGuest> _guestRepository = guestRepository;

        IPersistRepository<HotelGuestFaceCapture> _hotelGuestFaceCapturePersistRepository = hotelGuestFaceCapturePersistRepository;

        // ✅ DRY helper method
        private static async Task<T> PersistSingleAsync<T>(IPersistRepository<T> repository, T entity) where T : class
        {
            var result = await repository.AddOrUpdateAllAsync(new List<T> { entity });
            return result.First();
        }

        public Task<HotelGuest> Persist(HotelGuest guest) =>
            PersistSingleAsync(_guestRepository, guest);

        public Task<HotelGuest> UpdateAadharStatus(UpdateAadharStatusParam param) =>
           _guestRepository.UpdatePartialAsync(new HotelGuest() { Id = param.Id, VerificationStatus = param.VerificationStatus}, x => x.VerificationStatus);


        public Task<HotelGuestFaceCapture> PersistFaceCapture(HotelGuestFaceCapture hotelGuestFaceCapture) =>
            PersistSingleAsync(_hotelGuestFaceCapturePersistRepository, hotelGuestFaceCapture);
    }
}
