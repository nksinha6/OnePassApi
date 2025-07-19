using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OnePass.Infrastructure.Persistence
{
    public static class ModelBuilderExtensions
    {
        public static void UseSnakeCaseNames(this ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
            {
                // Table name
                if (entity.GetTableName() is string tableName)
                    entity.SetTableName(tableName.ToSnakeCase());

                // Columns
                foreach (var property in entity.GetProperties())
                {
                    if (property.Name is string propName)
                        property.SetColumnName(propName.ToSnakeCase());
                }

                // Keys
                foreach (var key in entity.GetKeys())
                {
                    if (key.GetName() is string keyName)
                        key.SetName(keyName.ToSnakeCase());
                }

                // Foreign keys
                foreach (var fk in entity.GetForeignKeys())
                {
                    if (fk.GetConstraintName() is string fkName)
                        fk.SetConstraintName(fkName.ToSnakeCase());
                }

                // Indexes
                foreach (var index in entity.GetIndexes())
                {
                    if (index.GetDatabaseName() is string indexName)
                        index.SetDatabaseName(indexName.ToSnakeCase());
                }
            }
        }

        private static string ToSnakeCase(this string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return name;

            var snake = Regex.Replace(
                name,
                @"([a-z0-9])([A-Z])",
                "$1_$2",
                RegexOptions.Compiled)
                .ToLowerInvariant();

            return snake;
        }
    }
}
