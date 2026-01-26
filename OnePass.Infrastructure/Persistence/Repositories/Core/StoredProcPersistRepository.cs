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
    public class StoredProcPersistRepository<TCommand> : IStoredProcPersistRepository<TCommand>
        where TCommand : class
    {
        private readonly NpgsqlConnection _connection;
        private readonly ILogger<StoredProcPersistRepository<TCommand>> _logger;
        private readonly Tracer _tracer;
        private readonly Dictionary<Type, string> _commandMappings;

        public StoredProcPersistRepository(
            NpgsqlConnection connection,
            Tracer tracer,
            ILogger<StoredProcPersistRepository<TCommand>> logger)
        {
            _connection = connection;
            _tracer = tracer;
            _logger = logger;

            _commandMappings = new Dictionary<Type, string>
            {
                { typeof(IUnitOfWork), "upsert_block_aggregator" },
                { typeof(DeleteGuestParam), "delete_guest_data_by_phone" }
            };
        }

        public async Task<bool> ExecuteCommandAsync(TCommand command)
        {
            if (!_commandMappings.TryGetValue(typeof(TCommand), out var routineName))
                throw new ArgumentException(
                    $"No postgres routine mapping for command type: {typeof(TCommand).Name}");

            using var span = _tracer.StartSpan($"PG:{routineName}", SpanKind.Internal);
            span.SetAttribute("command.type", typeof(TCommand).Name);
            span.SetAttribute("pg.routine", routineName);

            try
            {
                await ExecutePostgresRoutineAsync(routineName, command, span);
                span.SetAttribute("pg.success", true);
                return true;
            }
            catch (Exception ex)
            {
                span.RecordException(ex);
                _logger.LogError(ex, "Postgres routine execution failed: {Routine}", routineName);
                throw;
            }
            finally
            {
                span.End();
            }
        }

        private async Task ExecutePostgresRoutineAsync(
            string routineName,
            object command,
            TelemetrySpan span)
        {
            using var cmd = _connection.CreateCommand();

            var parameters = AddParameters(cmd, command, span);

            // Build named-parameter CALL (Postgres 11+ procedures)
            var paramList = string.Join(", ",
                parameters.Select(p => $"{p.ParameterName} := @{p.ParameterName}"));

            cmd.CommandText = $"CALL {routineName}({paramList})";
            cmd.CommandType = CommandType.Text;

            if (_connection.State != ConnectionState.Open)
                await ((NpgsqlConnection)_connection).OpenAsync();

            await ((NpgsqlCommand)cmd).ExecuteNonQueryAsync();
        }

        private List<IDbDataParameter> AddParameters(
            IDbCommand cmd,
            object command,
            TelemetrySpan span)
        {
            var parameters = new List<IDbDataParameter>();

            foreach (var prop in command.GetType()
                                        .GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var value = prop.GetValue(command) ?? DBNull.Value;

                var param = cmd.CreateParameter();
                param.ParameterName = prop.Name; // NO @ here
                param.Value = value;

                cmd.Parameters.Add(param);
                parameters.Add(param);

                span?.SetAttribute($"param.{prop.Name}", value?.ToString() ?? "null");
            }

            return parameters;
        }
    }
}
