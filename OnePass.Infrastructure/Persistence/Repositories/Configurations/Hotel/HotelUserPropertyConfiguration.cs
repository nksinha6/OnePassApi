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
    public class HotelUserPropertyConfiguration
        : IEntityTypeConfiguration<HotelUserProperty>
    {
        public void Configure(EntityTypeBuilder<HotelUserProperty> builder)
        {
            builder.ToTable("hotel_user_properties");

            builder.HasKey(x => new
            {
                x.UserId,
                x.TenantId,
                x.PropertyId
            });

            builder.Property(x => x.UserId)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(x => x.TenantId)
                   .IsRequired();

            builder.Property(x => x.PropertyId)
                   .IsRequired();

            // 🔹 INDEX (important)
            builder.HasIndex(x => new { x.UserId, x.TenantId });

            builder.HasOne<HotelUser>()
                   .WithMany()
                   .HasForeignKey(x => new { x.UserId, x.TenantId })
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<HotelProperty>()
                   .WithMany()
                   .HasForeignKey(x => x.PropertyId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
