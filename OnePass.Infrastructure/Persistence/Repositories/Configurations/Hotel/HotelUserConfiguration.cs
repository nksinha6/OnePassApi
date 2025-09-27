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
    public class HotelUserConfiguration : IEntityTypeConfiguration<HotelUser>
    {
        public void Configure(EntityTypeBuilder<HotelUser> builder)
        {
            builder.ToTable("hotel_users");

            // Composite primary key
            builder.HasKey(u => new { u.Id, u.TenantId });

            builder.Property(u => u.Id)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(u => u.TenantId)
                   .IsRequired();

            // Foreign key to hotel_tenant
            builder.HasOne<HotelTenant>() // no navigation property
                   .WithMany()
                   .HasForeignKey(u => u.TenantId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
