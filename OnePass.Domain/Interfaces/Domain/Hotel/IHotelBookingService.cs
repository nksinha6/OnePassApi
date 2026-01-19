using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public interface IHotelBookingService
    {
        public Task<HotelBookingMetadata> StartBookingVerification(HotelBookingMetadata request);
        public Task<HotelBookingMetadata> EndBookingVerification(int tenantId, int propertyId,  string bookingId);
        public Task<HotelPendingFaceMatch> RecordBookingPendingFaceVerification(int tenantId, int propertyId, FaceMatchInitiateRequest faceMatchInitiateRequest);

        Task<HotelPendingFaceMatch> VerifyBookingPendingFaceVerification(string bookingId, long id, byte[] bytes, string contentType, long length, string? latitude, string? longitude, string phoneCountryCode, string phoneNumber);
    }
}
