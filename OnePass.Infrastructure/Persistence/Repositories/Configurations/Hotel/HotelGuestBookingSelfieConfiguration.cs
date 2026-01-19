using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Infrastructure.Persistence
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using OnePass.Domain;

    public class HotelGuestBookingSelfieConfiguration
        : IEntityTypeConfiguration<HotelGuestBookingSelfie>
    {
        public void Configure(EntityTypeBuilder<HotelGuestBookingSelfie> builder)
        {
            builder.ToTable("hotel_guest_booking_selfies");

            builder.HasKey(x => new
            {
                x.BookingId,
                x.PhoneCountryCode,
                x.PhoneNumber
            });

            builder.Property(x => x.BookingId)
                .HasColumnName("booking_id")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.PhoneCountryCode)
                .HasColumnName("phone_country_code")
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(x => x.PhoneNumber)
                .HasColumnName("phone_number")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.Latitude).HasColumnName("latitude")
           .HasMaxLength(20);

            builder.Property(x => x.Longitude).HasColumnName("longitude")
                .HasMaxLength(20);

            builder.Property(x => x.Image)
                .HasColumnName("image")
                .IsRequired();

            builder.Property(x => x.ContentType)
                .HasColumnName("content_type")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.FileSize)
                .HasColumnName("file_size")
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at");
        }
    }
}
