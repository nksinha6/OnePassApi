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
    public class HotelPendingFaceMatchConfiguration : IEntityTypeConfiguration<HotelPendingFaceMatch>
    {
        public void Configure(EntityTypeBuilder<HotelPendingFaceMatch> builder)
        {
            builder.ToTable("hotel_pending_face_matches");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.BookingId)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(e => e.PhoneCountryCode)
                   .IsRequired()
                   .HasMaxLength(10);

            builder.Property(e => e.PhoneNumber)
                   .IsRequired()
                   .HasMaxLength(20);


            builder.Property(x => x.TenantId)
                   .IsRequired();

            builder.Property(x => x.PropertyId)
                   .IsRequired();

            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("now()");

            builder.Property(x => x.Status)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasDefaultValue("pending");

            // Indexes
            builder.HasIndex(x => new { x.TenantId, x.PropertyId });
            builder.HasIndex(x => x.BookingId);
        }
    }
}
