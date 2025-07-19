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
    public class PremiseTypeConfiguration : IEntityTypeConfiguration<PremiseType>
    {
        public void Configure(EntityTypeBuilder<PremiseType> builder)
        {
            builder.ToTable("premise_type");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                   .HasColumnName("id");

            builder.Property(e => e.Name)
                   .IsRequired()
                   .HasColumnName("name");

            builder.HasIndex(e => e.Name)
                   .IsUnique();
        }
    }
}
