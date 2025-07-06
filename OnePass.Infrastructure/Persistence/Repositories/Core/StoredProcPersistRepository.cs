using System;
using System.Collections.Generic;
using System.Data;
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
    public class StoredProcPersistRepository<TCommand> : IStoredProcPersistRepository<TCommand>
    where TCommand : class
    {
        private readonly SqlConnection _connection;
        private readonly ILogger<StoredProcPersistRepository<TCommand>> _logger;
        private readonly Tracer _tracer;
        private readonly Dictionary<Type, string> _commandMappings;

        public StoredProcPersistRepository(SqlConnection connection, Tracer tracer, ILogger<StoredProcPersistRepository<TCommand>> logger)
        {
            _connection = connection;
            _tracer = tracer;
            _logger = logger;

            _commandMappings = new Dictionary<Type, string>
        {
            { typeof(IUnitOfWork), "UpsertBlockAggregator" }
        };
        }

        public async Task<bool> ExecuteCommandAsync(TCommand command)
        {
            if (!_commandMappings.TryGetValue(typeof(TCommand), out var storedProcedure))
                throw new ArgumentException($"No stored procedure mapping for command type: {typeof(TCommand).Name}");

            using var span = _tracer.StartSpan($"SP:{storedProcedure}", SpanKind.Internal);
            span.SetAttribute("command.type", typeof(TCommand).Name);
            span.SetAttribute("sp.name", storedProcedure);

            try
            {
                await ExecuteStoredProcAsync(storedProcedure, command, span);
                span.SetAttribute("sp.success", true);
                span.End();
                return true;
            }
            catch (Exception ex)
            {
                span.RecordException(ex);
                _logger.LogError(ex, "Stored procedure execution failed: {Procedure}", storedProcedure);
                span.End();
                throw;
            }
        }

        private async Task ExecuteStoredProcAsync(string sp, object command, TelemetrySpan span)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = sp;
            cmd.CommandType = CommandType.StoredProcedure;
            AddParameters(cmd, command, span);

            if (_connection.State != ConnectionState.Open)
                await _connection.OpenAsync();

            await cmd.ExecuteNonQueryAsync();
        }

        private void AddParameters(SqlCommand cmd, object command, TelemetrySpan span)
        {
            foreach (var prop in command.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var paramName = $"@{prop.Name}";
                var value = prop.GetValue(command) ?? DBNull.Value;

                cmd.Parameters.AddWithValue(paramName, value);
                span.SetAttribute($"param.{prop.Name}", value.ToString() ?? "null");
            }
        }
    }

    }
