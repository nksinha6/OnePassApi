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

    public class HotelPendingQrCodeMatchConfiguration
        : IEntityTypeConfiguration<HotelPendingQrCodeMatch>
    {
        public void Configure(EntityTypeBuilder<HotelPendingQrCodeMatch> builder)
        {
            builder.ToTable("hotel_pending_qr_code_matches");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("id")
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.TenantId)
                   .HasColumnName("tenant_id")
                   .IsRequired();

            builder.Property(x => x.PropertyId)
                   .HasColumnName("property_id")
                   .IsRequired();

            builder.Property(x => x.BookingId)
                   .HasColumnName("booking_id")
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.PhoneCountryCode)
                   .HasColumnName("phone_country_code")
                   .HasMaxLength(5)
                   .IsRequired();

            builder.Property(x => x.PhoneNumber)
                   .HasColumnName("phone_number")
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(x => x.Status)
                   .HasColumnName("status")
                   .HasDefaultValue(PendingQrStatus.pending)
                   .IsRequired();

            builder.Property(x => x.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("NOW()")
                   .IsRequired();

            builder.HasIndex(x => new
            {
                x.TenantId,
                x.PropertyId,
                x.BookingId,
                x.PhoneCountryCode,
                x.PhoneNumber
            })
            .HasDatabaseName("uq_pending_qr_guest")
            .IsUnique();

            builder.HasOne<HotelTenant>()
                   .WithMany()
                   .HasForeignKey(x => x.TenantId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<HotelProperty>()
                   .WithMany()
                   .HasForeignKey(x => x.PropertyId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
