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
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("roles");

            builder.HasKey(r => r.RoleId);

            builder.Property(r => r.RoleId)
                   .HasColumnName("role_id")
                   .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(r => r.Name)
                   .HasColumnName("name")
                   .HasMaxLength(50)
                   .IsRequired();

            builder.HasIndex(r => r.Name)
                   .IsUnique();

            builder.Property(r => r.Description)
                   .HasColumnName("description");

            builder.Property(r => r.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
