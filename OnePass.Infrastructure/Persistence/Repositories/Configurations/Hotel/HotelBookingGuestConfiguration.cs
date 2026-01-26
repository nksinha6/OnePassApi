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
                .IsRequired();

            builder.Property(x => x.PropertyId)
                .IsRequired();

            builder.Property(x => x.BookingId)
                .IsRequired()
                .HasMaxLength(64);

            builder.Property(x => x.PhoneCountryCode)
                .IsRequired()
                .HasMaxLength(5);

            builder.Property(x => x.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("now()");
        }
    }

}
