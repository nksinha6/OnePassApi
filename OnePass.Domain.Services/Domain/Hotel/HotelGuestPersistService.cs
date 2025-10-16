
namespace OnePass.Domain.Services
{
    public class HotelGuestPersistService(IPersistRepository<HotelGuest> guestRepository) : IHotelGuestPersistService
    {
        private readonly IPersistRepository<HotelGuest> _guestRepository = guestRepository;

        // ✅ DRY helper method
        private static async Task<T> PersistSingleAsync<T>(IPersistRepository<T> repository, T entity) where T : class
        {
            var result = await repository.AddOrUpdateAllAsync(new List<T> { entity });
            return result.First();
        }

        public Task<HotelGuest> Persist(HotelGuest guest) =>
            PersistSingleAsync(_guestRepository, guest);
    }
}
