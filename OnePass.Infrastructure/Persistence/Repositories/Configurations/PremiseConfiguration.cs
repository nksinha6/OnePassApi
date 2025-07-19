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
    public class PremiseConfiguration : IEntityTypeConfiguration<Premise>
    {
        public void Configure(EntityTypeBuilder<Premise> builder)
        {
            builder.ToTable("premise");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id);

            builder.Property(e => e.TenantId)
               .IsRequired();

            builder.Property(e => e.Name)
                   .IsRequired();

            builder.Property(e => e.TypeId)
                   .IsRequired();

            builder.Property(e => e.ParentId);

            builder.Property(e => e.Latitude);

            builder.Property(e => e.Longitude);

            builder.Property(e => e.Address);
            builder.Property(e => e.City);
            builder.Property(e => e.Zip);
            builder.Property(e => e.State);
            builder.Property(e => e.Country);
            builder.Property(e => e.GmapUrl);
            builder.Property(e => e.Admin);

            builder.HasCheckConstraint(
                "chk_campus_no_parent",
                "NOT (type_id = 1 AND parent_id IS NOT NULL)"
            );

            builder.HasIndex(e => e.ParentId)
                   .HasDatabaseName("idx_premise_parent");

            builder.HasIndex(e => new { e.Latitude, e.Longitude })
                   .HasDatabaseName("idx_premise_lat_lng");

            builder.HasIndex(e => e.TenantId)
              .HasDatabaseName("idx_premise_tenant");
        }
    }

}
