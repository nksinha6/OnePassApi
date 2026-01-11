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
    public class BookingVerificationWindowConfiguration : IEntityTypeConfiguration<BookingVerificationWindow>
    {
        public void Configure(EntityTypeBuilder<BookingVerificationWindow> builder)
        {
            builder.ToTable("booking_verification_window");

            // Composite primary key
            builder.HasKey(b => new { b.TenantId, b.BookingId });

            builder.Property(b => b.TenantId).IsRequired();
            builder.Property(b => b.BookingId).IsRequired();
            builder.Property(b => b.WindowStart).IsRequired();
            builder.Property(b => b.WindowEnd); // optional

            // Foreign key to hotel_tenant (no navigation)
            builder.HasOne<HotelTenant>()
                   .WithMany()
                   .HasForeignKey(b => b.TenantId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Index on tenant_id for fast queries
            builder.HasIndex(b => b.TenantId);
        }
    }
}
