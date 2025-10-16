using System.Linq.Expressions;

namespace OnePass.Domain
{
    public interface IHotelGuestPersistService
    {
        Task<HotelGuest> Persist(HotelGuest guest);
        Task<HotelGuest> UpdateAadharStatus(UpdateAadharStatusParam param);
    }
}
