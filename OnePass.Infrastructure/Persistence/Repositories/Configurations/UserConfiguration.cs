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
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Table
            builder.ToTable("users");

            // Primary Key
            builder.HasKey(u => u.Phone);

            // Columns
            builder.Property(u => u.Phone)
                .HasColumnName("phone")
                .HasColumnType("VARCHAR(20)")
                .IsRequired();

            builder.Property(u => u.FirstName)
                .HasColumnName("first_name")
                .HasColumnType("VARCHAR(100)")
                .IsRequired();

            builder.Property(u => u.LastName)
                .HasColumnName("last_name")
                .HasColumnType("VARCHAR(100)");

            builder.Property(u => u.Email)
                .HasColumnName("email")
                .HasColumnType("VARCHAR(150)");

            // Unique index for Email
            builder.HasIndex(u => u.Email).IsUnique();

            builder.Property(u => u.Status)
                .HasColumnName("status")
                .HasColumnType("VARCHAR(20)")
                .HasDefaultValue("unverified")
                .IsRequired();

            builder.Property(u => u.IsEmailVerified)
                .HasColumnName("is_email_verified")
                .HasColumnType("BOOLEAN")
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(u => u.CreatedAt)
                .HasColumnName("created_at")
                 .HasColumnType("timestamptz")
                 .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // ✅ CHECK constraint for status
            builder.HasCheckConstraint("chk_users_status", "status IN ('unverified', 'created', 'registered', 'verified')");

            // ✅ CHECK constraint for phone format (PostgreSQL regex)
            builder.HasCheckConstraint("chk_users_phone_format",
                "phone ~ '^\\+?[0-9]{1,3}?[-. (]?[0-9]{1,4}?[-. )]?[0-9]{3,4}?[-. ]?[0-9]{3,4}$'");
        }
    }

}
