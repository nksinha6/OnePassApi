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
                   .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(ucr => ucr.UserPhone)
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(ucr => ucr.CompanyId)
                   .IsRequired();

            builder.Property(ucr => ucr.UnitId)
                   .IsRequired();

            builder.Property(ucr => ucr.RoleId)
                   .IsRequired();

            builder.Property(ucr => ucr.IsActive)
                   .HasDefaultValue(true);

            builder.Property(ucr => ucr.CreatedAt)
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Index on user_phone for fast queries
            builder.HasIndex(ucr => ucr.UserPhone)
                   .HasDatabaseName("idx_user_company_roles_user_phone");
        }
    }
}
