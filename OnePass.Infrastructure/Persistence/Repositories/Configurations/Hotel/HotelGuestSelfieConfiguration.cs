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

            builder.HasKey(x => new { x.PhoneCode, x.PhoneNumber });

            builder.Property(x => x.PhoneCode)
                   .IsRequired()
                   .HasMaxLength(10);

            builder.Property(x => x.PhoneNumber)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(x => x.Selfie)
                   .IsRequired();

            builder.Property(x => x.CreatedAt)
                   .IsRequired();

            builder.Property(x => x.UpdatedAt);
        }
    }

}
