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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.HasKey(u => u.UserId);

            builder.Property(u => u.UserId)
                   .HasColumnName("user_id")
                   .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(u => u.Name)
                   .HasColumnName("name")
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(u => u.Email)
                   .HasColumnName("email")
                   .HasMaxLength(150)
                   .IsRequired();

            builder.Property(u => u.Phone)
                   .HasColumnName("phone")
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(u => u.Status)
                   .HasColumnName("status")
                   .HasMaxLength(20)
                   .HasDefaultValue("unverified")
                   .IsRequired();

            builder.Property(u => u.IsEmailVerified)
                   .HasColumnName("is_email_verified")
                   .HasDefaultValue(false)
                   .IsRequired();

            builder.Property(u => u.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP")
                   .IsRequired();

            // Optional: enforce allowed values in code
            builder.HasCheckConstraint("CK_users_status",
                "status IN ('unverified', 'created', 'registered', 'verified')");
        }
    }

}
