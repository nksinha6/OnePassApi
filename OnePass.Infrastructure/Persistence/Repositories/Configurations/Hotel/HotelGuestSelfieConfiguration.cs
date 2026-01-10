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

    public class HotelGuestSelfieConfiguration : IEntityTypeConfiguration<HotelGuestSelfie>
    {
        public void Configure(EntityTypeBuilder<HotelGuestSelfie> builder)
        {
            builder.ToTable("hotel_guest_selfies");

            // Composite primary key
            builder.HasKey(x => new
            {
                x.PhoneCountryCode,
                x.PhoneNumber
            });

            builder.Property(x => x.PhoneCountryCode)
                .HasColumnName("phone_country_code")
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(x => x.PhoneNumber)
                .HasColumnName("phone_number")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.ImageOid)
                .HasColumnName("image_oid")
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
                .HasDefaultValueSql("NOW()")
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at");
        }
    }

}
