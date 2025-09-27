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
    public class HotelUserRefreshTokenConfiguration : IEntityTypeConfiguration<HotelUserRefreshToken>
    {
        public void Configure(EntityTypeBuilder<HotelUserRefreshToken> builder)
        {
            builder.ToTable("hotel_user_refresh_tokens");

            builder.HasKey(x => x.Token);

            builder.Property(x => x.Token)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.UserId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.TenantId)
                .IsRequired();

            builder.Property(x => x.ExpiresAt)
                .IsRequired();

            builder.Property(x => x.IsRevoked)
                .IsRequired()
                .HasDefaultValue(false);
        }
    }
}
