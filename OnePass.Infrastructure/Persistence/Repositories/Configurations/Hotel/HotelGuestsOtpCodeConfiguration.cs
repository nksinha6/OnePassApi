using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnePass.Domain;

namespace OnePass.Infrastructure.Persistence
{
    public class HotelGuestsOtpCodeConfiguration : IEntityTypeConfiguration<HotelGuestsOtpCode>
    {
        public void Configure(EntityTypeBuilder<HotelGuestsOtpCode> builder)
        {
            // Specify composite primary key
            builder.HasKey(x => new { x.PhoneCountryCode, x.PhoneNumber });

            // Configure required fields & defaults (optional but recommended)
            builder.Property(x => x.HashedOtp)
                   .IsRequired();

            builder.Property(x => x.CreatedAt)
                   .IsRequired();

            builder.Property(x => x.ExpiresAt)
                   .IsRequired();

            builder.Property(x => x.Attempts)
                   .IsRequired();

            builder.Property(x => x.IsUsed)
                   .IsRequired();
        }
    }
}
