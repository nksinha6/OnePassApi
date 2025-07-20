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
    public class PropertyConfiguration : IEntityTypeConfiguration<Property>
    {
        public void Configure(EntityTypeBuilder<Property> builder)
        {
            builder.ToTable("property");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired();

            builder.Property(p => p.CompanyId)
                .IsRequired();

            builder.Property(p => p.Address);

            builder.Property(p => p.City);

            builder.Property(p => p.Pincode);

            builder.Property(p => p.State);

            builder.Property(p => p.Country);

            builder.Property(p => p.GmapUrl);

            builder.Property(p => p.AdminPhone);

            builder.HasCheckConstraint(
                "CK_Property_AdminPhone_Format",
                @"admin_phone ~ '^(\+)?[0-9\s\-().]{7,25}$'"
            );

        }
    }
}
