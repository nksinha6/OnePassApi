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
    public class HotelTenantConfiguration : IEntityTypeConfiguration<HotelTenant>
    {
        public void Configure(EntityTypeBuilder<HotelTenant> builder)
        {
            builder.ToTable("hotel_tenant");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(x => x.Logo);

            builder.Property(x => x.LogoContentType)
                .HasMaxLength(50);
        }
    }
}
