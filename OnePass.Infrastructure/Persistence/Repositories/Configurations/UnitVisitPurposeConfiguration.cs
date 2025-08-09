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

    public class UnitVisitPurposeConfiguration : IEntityTypeConfiguration<UnitVisitPurpose>
    {
        public void Configure(EntityTypeBuilder<UnitVisitPurpose> builder)
        {
            builder.ToTable("unit_visit_purposes");

            builder.HasKey(u => u.Id);

            builder.HasIndex(u => new { u.UnitId, u.VisitPurposeId })
                .IsUnique();

            builder.HasCheckConstraint(
                "chk_uvp_verification_requires_approval",
                "is_verification_required IS NULL OR is_verification_required = false OR (is_verification_required = true AND is_host_approval_required = true)"
            );
        }
    }
}

