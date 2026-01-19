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

    public sealed class HotelBookingMetadataConfiguration
        : IEntityTypeConfiguration<HotelBookingMetadata>
    {
        public void Configure(EntityTypeBuilder<HotelBookingMetadata> builder)
        {
            builder.ToTable("hotel_booking_metadata");

            // Composite Primary Key
            builder.HasKey(x => new
            {
                x.TenantId,
                x.PropertyId,
                x.BookingId
            });

            builder.Property(x => x.BookingId)
                .IsRequired();

            builder.Property(x => x.Ota)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(x => x.PhoneCountryCode)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(x => x.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.AdultsCount)
                .IsRequired();

            builder.Property(x => x.MinorsCount)
                .IsRequired();

            builder.Property(x => x.WindowStart)
                .IsRequired();

            builder.Property(x => x.WindowEnd);

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("NOW()")
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(x => x.UpdatedAt);

            // Index: (tenant_id, property_id)
            builder.HasIndex(x => new
            {
                x.TenantId,
                x.PropertyId
            });
        }
    }
}
