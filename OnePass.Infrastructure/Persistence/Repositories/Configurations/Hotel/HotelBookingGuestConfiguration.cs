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

    public class HotelBookingGuestConfiguration
        : IEntityTypeConfiguration<HotelBookingGuest>
    {
        public void Configure(EntityTypeBuilder<HotelBookingGuest> builder)
        {
            builder.ToTable("hotel_booking_guests");

            // Composite Primary Key
            builder.HasKey(x => new
            {
                x.TenantId,
                x.PropertyId,
                x.BookingId,
                x.PhoneCountryCode,
                x.PhoneNumber
            });

            builder.Property(x => x.TenantId)
                         .HasColumnName("tenant_id")
                .IsRequired();

            builder.Property(x => x.BookingId)
                .HasColumnName("booking_id")
                .IsRequired()
                .HasMaxLength(64);

            builder.Property(x => x.PropertyId)
                  .HasColumnName("property_id")
                  .IsRequired();

            builder.Property(x => x.PhoneCountryCode)
                .HasColumnName("phone_country_code")
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(x => x.PhoneNumber)
                .HasColumnName("phone_number")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired()
                .HasDefaultValueSql("now()");
        }
    }

}
