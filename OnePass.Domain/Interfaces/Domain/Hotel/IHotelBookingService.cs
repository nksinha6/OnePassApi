using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public interface IHotelBookingService
    {
        public Task<BookingCheckin> StartBookingCheckin(int tenantId, string bookingId);
        public Task<BookingCheckin> RecordBookingCheckin(int tenantId, string bookingId);
        public Task<HotelPendingFaceMatch> RecordBookingPendingFaceVerification(int tenantId, int propertyId, FaceMatchInitiateRequest faceMatchInitiateRequest);

        Task<HotelPendingFaceMatch> VerifyBookingPendingFaceVerification(long id);
    }
}
