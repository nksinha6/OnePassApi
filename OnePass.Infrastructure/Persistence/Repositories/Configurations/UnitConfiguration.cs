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
            // ✅ Table name
            builder.ToTable("units");

            // ✅ Primary key
            builder.HasKey(u => u.Id);

            // ✅ Fields
            builder.Property(u => u.Id)
                   .IsRequired();

            builder.Property(u => u.Name)
                   .IsRequired();

            builder.Property(u => u.CompanyId)
                   .IsRequired();

            builder.Property(u => u.PropertyId)
                   .IsRequired(false);

            builder.Property(u => u.Floor)
                   .IsRequired(false);

            builder.Property(u => u.AdminPhone)
                   .IsRequired(false);

            // ✅ Foreign Keys (no navigation props)
            builder.HasOne<Company>()
                   .WithMany()
                   .HasForeignKey(u => u.CompanyId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Property>()
                   .WithMany()
                   .HasForeignKey(u => u.PropertyId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(u => u.AdminPhone)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
