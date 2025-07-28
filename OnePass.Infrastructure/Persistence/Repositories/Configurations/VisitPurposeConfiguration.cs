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
    public class VisitPurposeConfiguration : IEntityTypeConfiguration<VisitPurpose>
    {
        public void Configure(EntityTypeBuilder<VisitPurpose> builder)
        {
            builder.ToTable("visit_purposes");

            // Primary Key
            builder.HasKey(vp => vp.Id);

            // Properties
            builder.Property(vp => vp.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(vp => vp.Description)
                .IsRequired(false);

            builder.Property(vp => vp.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
