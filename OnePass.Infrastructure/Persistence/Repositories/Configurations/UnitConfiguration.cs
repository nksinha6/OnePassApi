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

    public class UnitConfiguration : IEntityTypeConfiguration<Unit>
    {
        public void Configure(EntityTypeBuilder<Unit> builder)
        {
            // table name
            builder.ToTable("unit");

            // primary key
            builder.HasKey(u => u.Id);

            // required fields
            builder.Property(u => u.Id).IsRequired();

            builder.Property(u => u.Name).IsRequired();

            builder.Property(u => u.CompanyId).IsRequired();

            // optional fields
            builder.Property(u => u.PropertyId);

            builder.Property(u => u.Floor);

            builder.Property(u => u.AdminPhone);

            // CHECK constraint
            builder.HasCheckConstraint(
                "CK_Unit_AdminPhone_Format",
                @"admin_phone ~ '^(\+)?[0-9\s\-().]{7,25}$'"
            );

            // foreign keys without navigation properties
            builder.HasOne<Company>()
                   .WithMany()
                   .HasForeignKey(u => u.CompanyId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Property>()
                   .WithMany()
                   .HasForeignKey(u => u.PropertyId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
