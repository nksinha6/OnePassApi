using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OnePass.Infrastructure.Persistence
{
    public class OnePassDbContext : DbContext
    {
        public OnePassDbContext(DbContextOptions<OnePassDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply all IEntityTypeConfiguration<T> classes from this assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Apply lowercase naming convention globally (for PostgreSQL)
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Table name
                entity.SetTableName(entity.GetTableName()!.ToLower());

                // Columns
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.Name.ToLower());
                }

                // Keys (primary)
                foreach (var key in entity.GetKeys())
                {
                    key.SetName(key.GetName()!.ToLower());
                }

                // Foreign keys
                foreach (var foreignKey in entity.GetForeignKeys())
                {
                    foreignKey.SetConstraintName(foreignKey.GetConstraintName()!.ToLower());
                }

                // Indexes
                foreach (var index in entity.GetIndexes())
                {
                    index.SetDatabaseName(index.GetDatabaseName()!.ToLower());
                }
            }
        }
    }
}
