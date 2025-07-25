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
    public class UserCompanyRoleConfiguration : IEntityTypeConfiguration<UserCompanyRole>
    {
        public void Configure(EntityTypeBuilder<UserCompanyRole> builder)
        {
            builder.ToTable("user_company_roles");

            // Primary Key
            builder.HasKey(ucr => ucr.Id);

            builder.Property(ucr => ucr.Id)
                   .HasColumnName("id")
                   .HasDefaultValueSql("gen_random_uuid()");

            // Foreign Key columns (no navigation properties)
            builder.Property(ucr => ucr.UserId)
                   .HasColumnName("user_id")
                   .IsRequired();

            builder.Property(ucr => ucr.CompanyId)
                   .HasColumnName("company_id")
                   .IsRequired();

            builder.Property(ucr => ucr.UnitId)
                   .HasColumnName("unit_id")
                   .IsRequired();

            builder.Property(ucr => ucr.RoleId)
                   .HasColumnName("role_id")
                   .IsRequired();

            // Active flag
            builder.Property(ucr => ucr.IsActive)
                   .HasColumnName("is_active")
                   .HasDefaultValue(true);

            // Timestamp
            builder.Property(ucr => ucr.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
