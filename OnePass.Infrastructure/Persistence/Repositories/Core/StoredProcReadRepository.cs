using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Npgsql;
using OnePass.Domain;
using OpenTelemetry.Trace;

namespace OnePass.Infrastructure.Persistence
{
    public class StoredProcReadRepository<TQuery, TResult> : IStoredProcReadRepository<TQuery, TResult>
        where TQuery : IReadQuery
        where TResult : class, new()
    {
        private readonly NpgsqlConnection _connection;
        private readonly ILogger<StoredProcReadRepository<TQuery, TResult>> _logger;
        private readonly Tracer _tracer;
        private readonly Dictionary<Type, string> _queryMappings;

        public StoredProcReadRepository(NpgsqlConnection connection, Tracer tracer, ILogger<StoredProcReadRepository<TQuery, TResult>> logger)
        {
            _connection = connection;
            _tracer = tracer;
            _logger = logger;

            // ✅ Map Query DTO -> Postgres Function Name
            _queryMappings = new Dictionary<Type, string>
            {
                { typeof(GetInviteByHostPhoneQuery), "get_host_invite_details" },
                { typeof(GetInvitesByGuestPhoneQuery), "get_guest_invite_details" }
            };
        }

        public async Task<IEnumerable<TResult>> ExecuteQueryAsync(TQuery query)
        {
            if (!_queryMappings.TryGetValue(typeof(TQuery), out var functionName))
                throw new ArgumentException($"No mapping found for query type: {typeof(TQuery).Name}");

            using var span = _tracer.StartSpan($"PGFUNC:{functionName}", SpanKind.Internal);
            span.SetAttribute("query.type", typeof(TQuery).Name);
            span.SetAttribute("pg.function", functionName);
            try
            {
                var result = await ExecuteFunctionAsync(functionName, query, span);
                span.SetAttribute("pg.success", true);
                return result;
            }
            catch (Exception ex)
            {
                span.RecordException(ex);
                _logger.LogError(ex, "Postgres function execution failed: {Function}", functionName);
                throw;
            }
            finally
            {
                span.End();
            }
        }

        private async Task<IEnumerable<TResult>> ExecuteFunctionAsync(string functionName, object query, TelemetrySpan span)
        {
            var results = new List<TResult>();

            // ✅ Instead of StoredProcedure, we SELECT from function
            var sql = $"SELECT * FROM {functionName}({BuildParameterList(query)})";

            using var cmd = new NpgsqlCommand(sql, _connection);
            AddParameters(cmd, query, span);

            if (_connection.State != ConnectionState.Open)
                await _connection.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
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

        // ✅ Build a parameter list like: @p_host_phone
        private string BuildParameterList(object query)
        {
            var props = query.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            return string.Join(", ", props.Select(p => $"@{p.Name}"));
        }

        // ✅ Add query params to command
        private void AddParameters(NpgsqlCommand cmd, object query, TelemetrySpan span)
        {
            foreach (var prop in query.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var paramName = $"@{prop.Name}";
                var value = prop.GetValue(query) ?? DBNull.Value;
                cmd.Parameters.AddWithValue(paramName, value);
                span.SetAttribute($"param.{prop.Name}", value?.ToString() ?? "null");
            }
        }

        // ✅ Map properties from reader to TResult
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

