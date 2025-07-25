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
            builder.ToTable("companies");

            builder.HasKey(c => c.CompanyId);

            builder.Property(c => c.CompanyId)
                   .HasColumnName("company_id")
                   .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(c => c.Name)
                   .HasColumnName("name")
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(c => c.Address)
                   .HasColumnName("address");

            builder.Property(c => c.Zip)
                   .HasColumnName("zip");

            builder.Property(c => c.City)
                   .HasColumnName("city");

            builder.Property(c => c.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }

}
