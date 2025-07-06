using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using OnePass.Domain;
using OpenTelemetry.Trace;

namespace OnePass.Infrastructure.Persistence
{
    public class StoredProcReadRepository<TQuery, TResult> : IStoredProcReadRepository<TQuery, TResult>
    where TQuery : IReadQuery
    where TResult : class, new()
    {
        private readonly SqlConnection _connection;
        private readonly ILogger<StoredProcReadRepository<TQuery, TResult>> _logger;
        private readonly Tracer _tracer;
        private readonly Dictionary<Type, string> _queryMappings;

        public StoredProcReadRepository(SqlConnection connection, Tracer tracer, ILogger<StoredProcReadRepository<TQuery, TResult>> logger)
        {
            _connection = connection;
            _tracer = tracer;
            _logger = logger;

            _queryMappings = new Dictionary<Type, string>
            {
                { typeof(IUnitOfWork), "GETDSLBySuperintendentId" }
            };
        }

        public async Task<IEnumerable<TResult>> ExecuteQueryAsync(TQuery query)
        {
            if (!_queryMappings.TryGetValue(typeof(TQuery), out var storedProcedure))
                throw new ArgumentException($"No stored procedure mapping for query type: {typeof(TQuery).Name}");

            using var span = _tracer.StartSpan($"SP:{storedProcedure}", SpanKind.Internal);
            span.SetAttribute("query.type", typeof(TQuery).Name);
            span.SetAttribute("sp.storedProcedure", storedProcedure);

            try
            {
                var result = await ExecuteStoredProcAsync(storedProcedure, query, span);
                span.SetAttribute("sp.success", true);
                span.End();
                return result;
            }
            catch (Exception ex)
            {
                span.RecordException(ex);
                _logger.LogError(ex, "Stored procedure execution failed: {Procedure}", storedProcedure);
                span.End();
                throw;
            }
        }

        private async Task<IEnumerable<TResult>> ExecuteStoredProcAsync(string sp, object query, TelemetrySpan span)
        {
            var results = new List<TResult>();

            using var command = _connection.CreateCommand();
            command.CommandText = sp;
            command.CommandType = CommandType.StoredProcedure;
            AddParameters(command, query, span);

            if (_connection.State != ConnectionState.Open)
                await _connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            var propertyMap = GetPropertyMap(typeof(TResult));

            while (await reader.ReadAsync())
            {
                var entity = new TResult();
                foreach (var kvp in propertyMap)
                {
                    if (reader.HasColumn(kvp.Key))
                    {
                        var value = reader[kvp.Key];
                        if (value is not DBNull)
                            kvp.Value.SetValue(entity, value);
                    }
                }
                results.Add(entity);
            }

            return results;
        }

        private void AddParameters(SqlCommand cmd, object query, TelemetrySpan span)
        {
            foreach (var prop in query.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var paramName = $"@{prop.Name}";
                var value = prop.GetValue(query) ?? DBNull.Value;
                cmd.Parameters.AddWithValue(paramName, value);
                span.SetAttribute($"param.{prop.Name}", value?.ToString() ?? "null");
            }
        }

        private Dictionary<string, PropertyInfo> GetPropertyMap(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                       .ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase);
        }

        public Task<IEnumerable<TResult>> ExecuteAllAsync()
        {
            throw new NotImplementedException("ExecuteAllAsync not implemented");
        }
    }

}
