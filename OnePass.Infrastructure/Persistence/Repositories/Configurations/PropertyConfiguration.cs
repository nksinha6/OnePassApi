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
            // ✅ Table
            builder.ToTable("properties");

            // ✅ Primary Key
            builder.HasKey(p => p.Id);

            // ✅ Fields
            builder.Property(p => p.Id)
                   .IsRequired();

            builder.Property(p => p.CompanyId)
                   .IsRequired();

            builder.Property(p => p.Name)
                   .IsRequired();

            builder.Property(p => p.Address)
                   .IsRequired(false);

            builder.Property(p => p.City)
                   .IsRequired(false);

            builder.Property(p => p.Pincode)
                   .IsRequired(false);

            builder.Property(p => p.State)
                   .IsRequired(false);

            builder.Property(p => p.Country)
                   .IsRequired(false);

            builder.Property(p => p.GmapUrl)
                   .IsRequired(false);

            builder.Property(p => p.AdminPhone)
                   .IsRequired();

            // ✅ Foreign Keys (still no navigation props)
            builder.HasOne<Company>()
                   .WithMany()
                   .HasForeignKey(p => p.CompanyId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(p => p.AdminPhone)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
