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
    public class LawEnforcementOfficeConfiguration : IEntityTypeConfiguration<LawEnforcementOffice>
    {
        public void Configure(EntityTypeBuilder<LawEnforcementOffice> builder)
        {
            builder.ToTable("law_enforcement_offices");

            builder.HasKey(l => l.Id);

            builder.Property(l => l.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(l => l.ContactPerson)
                .HasMaxLength(100);

            builder.Property(l => l.ContactPhone)
                .HasMaxLength(20);

            builder.Property(l => l.ContactEmail)
                .HasMaxLength(150);

            builder.Property(l => l.CreatedAt)
                .HasDefaultValueSql("now()");

            builder.Property(l => l.UpdatedAt)
                .HasDefaultValueSql("now()");
        }
    }
}
