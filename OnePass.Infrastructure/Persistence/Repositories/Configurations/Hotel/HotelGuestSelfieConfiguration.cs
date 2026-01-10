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

            builder.HasKey(x => new
            {
                x.PhoneCountryCode,
                x.PhoneNumber
            });

            builder.Property(x => x.PhoneCountryCode)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(x => x.PhoneNumber)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.Image)
                .HasColumnType("bytea")
                .IsRequired();

            builder.Property(x => x.ContentType)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.FileSize)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("now()")
                .IsRequired();
        }
    }

}
