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
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            // Map to table
            builder.ToTable("companies");

            // Primary key
            builder.HasKey(c => c.Id);

            // Column mappings
            builder.Property(c => c.Id)
                .HasColumnName("id")
                .IsRequired();

            builder.Property(c => c.Name)
                .HasColumnName("name")
                .IsRequired();

            builder.Property(c => c.Address).HasColumnName("address");
            builder.Property(c => c.City).HasColumnName("city");
            builder.Property(c => c.State).HasColumnName("state");
            builder.Property(c => c.Zip).HasColumnName("zip");
            builder.Property(c => c.Country).HasColumnName("country");
            builder.Property(c => c.Website).HasColumnName("website");
            builder.Property(c => c.Email).HasColumnName("email");
            builder.Property(c => c.Phone).HasColumnName("phone");

            builder.Property(c => c.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("now()");

            builder.Property(c => c.UpdatedAt)
                .HasColumnName("updated_at")
                .HasDefaultValueSql("now()");
        }
    }
}
