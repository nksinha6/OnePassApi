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
    public class HotelUserPasswordConfiguration : IEntityTypeConfiguration<HotelUserPassword>
    {
        public void Configure(EntityTypeBuilder<HotelUserPassword> builder)
        {
            builder.ToTable("hotel_user_passwords");

            // Composite primary key

            builder.HasKey(p => new { p.UserId, p.TenantId });

            builder.Property(p => p.UserId)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(p => p.TenantId)
                   .IsRequired();

            builder.Property(p => p.PasswordHash)
                   .HasMaxLength(255)
                   .IsRequired();
        }
    }
}
