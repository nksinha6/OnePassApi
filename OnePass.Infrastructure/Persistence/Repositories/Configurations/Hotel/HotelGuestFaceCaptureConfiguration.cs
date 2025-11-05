using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OnePass.Domain;

namespace OnePass.Infrastructure.Persistence
{
    public class HotelGuestFaceCaptureConfiguration : IEntityTypeConfiguration<HotelGuestFaceCapture>
    {
        public void Configure(EntityTypeBuilder<HotelGuestFaceCapture> entity)
        {
            entity.ToTable("hotel_guest_face_capture");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            entity.Property(e => e.TenantId)
          .IsRequired(); // ✅ new property

            entity.Property(e => e.BookingId)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.PhoneNo)
                .HasMaxLength(15)
                .IsRequired();

            entity.Property(e => e.GuestId)
                .HasMaxLength(50);

            entity.Property(e => e.LiveCaptureDatetime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();

            entity.Property(e => e.FaceImage);

            entity.Property(e => e.FaceMatchScore)
                .HasPrecision(5, 2);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();

            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();

            entity.HasIndex(e => new { e.BookingId, e.PhoneNo })
                .IsUnique()
                .HasDatabaseName("uq_booking_phone");
        }
    }
}
