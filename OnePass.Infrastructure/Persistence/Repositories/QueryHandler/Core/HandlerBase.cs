using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnePass.Domain;
using OpenTelemetry.Trace;

namespace OnePass.Infrastructure.Persistence
{
    public abstract class HandlerBase
    {
        private readonly Tracer _tracer;
        private readonly ILogger _logger;

        protected HandlerBase(Tracer tracer, ILogger<HandlerBase> logger)
        {
            _tracer = tracer;
            _logger = logger;
        }

        protected Task<IEnumerable<TResult>> ExecuteSafelyWithTracingAsync<TQuery, TResult>(
            TQuery query,
            Func<Task<IEnumerable<TResult>>> operation)
            where TQuery : IReadQuery
        {
            return ExecuteWithTracingAsync(query, operation);
        }

        private async Task<IEnumerable<T>> ExecuteWithTracingAsync<TQuery, T>(
            TQuery query,
            Func<Task<IEnumerable<T>>> operation)
            where TQuery : IReadQuery
        {
            using var span = _tracer.StartSpan("Database Operation", SpanKind.Client);
            span.SetAttribute("query.type", typeof(TQuery).Name);
            span.SetAttribute("query.data", query.ToString());

            try
            {
                var result = await operation();
                return result;
            }
            catch (DbUpdateException dbEx)
            {
                span.SetAttribute("status", "failure");
                span.SetAttribute(
                    "ef.failed_entities",
                    string.Join(",", dbEx.Entries.Select(e => e.Entity.GetType().Name))
                );

                span.RecordException(dbEx);
                _logger.LogError(dbEx, "Database operation failed");
                throw new DBExecutionException<TQuery>("Database operation failed", dbEx, query);
            }
            catch (SqlException sqlEx)
            {
                span.SetAttribute("status", "failure");
                span.SetAttribute("sql.error.number", sqlEx.Number);
                span.SetAttribute("sql.error.line", sqlEx.LineNumber);
                span.SetAttribute("sql.error.procedure", sqlEx.Procedure ?? "N/A");

                span.RecordException(sqlEx);
                _logger.LogError(sqlEx, "SQL error occurred");
                throw new DBExecutionException<TQuery>("SQL operation failed", sqlEx, query);
            }
            catch (Exception ex)
            {
                span.SetAttribute("status", "failure");

                span.RecordException(ex);
                _logger.LogError(ex, "Unexpected error occurred");
                throw new DBExecutionException<TQuery>("An unexpected error occurred", ex, query);
            }
        }
    }
}
