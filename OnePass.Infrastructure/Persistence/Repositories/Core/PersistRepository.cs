using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnePass.Domain;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace OnePass.Infrastructure.Persistence
{
    public class PersistRepository<T> : IPersistRepository<T> where T : class
    {
        private readonly OnePassDbContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly ILogger<PersistRepository<T>> _logger;
        private readonly Tracer _tracer;

        public PersistRepository(OnePassDbContext context, ILogger<PersistRepository<T>> logger, Tracer tracer)
        {
            _context = context;
            _dbSet = context.Set<T>();
            _logger = logger;
            _tracer = tracer;
        }

        #region Public CRUD Methods

        public async Task<T> AddAsync(T entity) =>
            await ExecuteDbOperation(entity, async () =>
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
            }).ContinueWith(_ => entity);

        public async Task UpdateAsync(T entity) =>
            await ExecuteDbOperation(entity, async () =>
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
            });

        public async Task DeleteAsync(T entity) =>
            await ExecuteDbOperation(entity, async () =>
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            });

        public async Task<T> AddOrUpdateAsync(T entity) =>
    await ExecuteDbOperation(entity, async () =>
    {
        await AddOrUpdateEntityAsync(entity);
        await _context.SaveChangesAsync();
    }).ContinueWith(_ => entity);

        public async Task<IEnumerable<T>> AddAllAsync(IEnumerable<T> entities) =>
            await ExecuteDbOperation(entities, async () =>
            {
                if (!HasEntities(entities)) return;
                await _dbSet.AddRangeAsync(entities);
                await _context.SaveChangesAsync();
            }).ContinueWith(_ => entities);

        public async Task UpdateAllAsync(IEnumerable<T> entities) =>
            await ExecuteDbOperation(entities, async () =>
            {
                if (!HasEntities(entities)) return;
                _dbSet.UpdateRange(entities);
                await _context.SaveChangesAsync();
            });

        public async Task DeleteAllAsync(IEnumerable<T> entities) =>
            await ExecuteDbOperation(entities, async () =>
            {
                if (!HasEntities(entities)) return;
                _dbSet.RemoveRange(entities);
                await _context.SaveChangesAsync();
            });

        public async Task<IEnumerable<T>> AddOrUpdateAllAsync(IEnumerable<T> entities) =>
            await ExecuteDbOperation(entities, async () =>
            {
                if (!HasEntities(entities)) return;

                foreach (var entity in entities)
                    await AddOrUpdateEntityAsync(entity);

                await _context.SaveChangesAsync();
            }).ContinueWith(_ => entities);

        public async Task<T> UpdatePartialAsync(T entity, params Expression<Func<T, object>>[] updatedProperties) =>
            await ExecuteDbOperation(entity, async () =>
            {
                _dbSet.Attach(entity);
                var entry = _context.Entry(entity);
                foreach (var property in updatedProperties)
                    entry.Property(property).IsModified = true;

                await _context.SaveChangesAsync();
                return entity;
            });

        /// <summary>
        /// Add entity only if it doesn’t exist (by primary key).
        /// Returns the existing entity if found, otherwise inserts a new one.
        /// </summary>
        public async Task<T> AddIfNotExistAsync(T entity) =>
            await ExecuteDbOperation<T, T>(entity, async () =>
            {
                var existing = await FindByPrimaryKeyAsync(entity);
                if (existing != null) return existing;

                SetCreatedAtIfAvailable(entity);
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            });

        #endregion

        #region Private Helpers

        /// <summary>
        /// Checks if an entity collection has items; logs if empty.
        /// </summary>
        private bool HasEntities(IEnumerable<T> entities)
        {
            if (entities == null || !entities.Any())
            {
                _logger.LogInformation("{Method} called with empty or null collection for type {EntityType}",
                    new StackTrace().GetFrame(1)?.GetMethod()?.Name ?? "Unknown",
                    typeof(T).Name);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Adds or updates a single entity based on primary key existence.
        /// </summary>
        private async Task AddOrUpdateEntityAsync(T entity)
        {
            var entry = _context.Entry(entity);

            if (entry.State != EntityState.Detached) return;

            var existing = await FindByPrimaryKeyAsync(entity);

            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(entity);
                PreserveOrSetCreatedAt(existing);
            }
            else
            {
                SetCreatedAtIfAvailable(entity);
                await _dbSet.AddAsync(entity);
            }
        }

        /// <summary>
        /// Looks up an entity in the database by its primary key values.
        /// </summary>
        private async Task<T> FindByPrimaryKeyAsync(T entity)
        {
            var key = _context.Model.FindEntityType(typeof(T))?.FindPrimaryKey()
                ?? throw new InvalidOperationException($"Entity {typeof(T).Name} does not have a defined primary key.");

            var keyValues = key.Properties.Select(p => p.PropertyInfo.GetValue(entity)).ToArray();
            return await _dbSet.FindAsync(keyValues);
        }

        /// <summary>
        /// Sets CreatedAt property to UtcNow if null/default.
        /// </summary>
        private void SetCreatedAtIfAvailable(T entity)
        {
            var createdAtProp = typeof(T).GetProperty("CreatedAt");
            if (createdAtProp != null && createdAtProp.CanWrite)
            {
                var currentValue = createdAtProp.GetValue(entity);
                if (currentValue == null || (currentValue is DateTime dt && dt == default))
                    createdAtProp.SetValue(entity, DateTime.UtcNow);
            }
        }

        /// <summary>
        /// Preserves CreatedAt if already set on the DB entity.
        /// </summary>
        private void PreserveOrSetCreatedAt(T entity)
        {
            var createdAtProp = typeof(T).GetProperty("CreatedAt");
            if (createdAtProp != null && createdAtProp.CanWrite)
            {
                var value = createdAtProp.GetValue(entity);
                createdAtProp.SetValue(entity, value ?? DateTime.UtcNow);
            }
        }

        #endregion

        #region ExecuteDbOperation Overloads

        private async Task ExecuteDbOperation<TReq>(TReq req, Func<Task> dbOperation)
        {
            var operation = GetCaller();
            var span = _tracer.StartSpan($"DB:{operation}", SpanKind.Client);
            span.SetAttribute("db.system", "sql");
            span.SetAttribute("db.operation", typeof(TReq).Name);

            try
            {
                await dbOperation();
            }
            catch (Exception ex)
            {
                   HandleDbException(ex, span, req);
                throw;
            }
            finally
            {
                span.End();
            }
        }

        private async Task<TResult> ExecuteDbOperation<TReq, TResult>(TReq req, Func<Task<TResult>> dbOperation)
        {
            var operation = GetCaller();
            var span = _tracer.StartSpan($"DB:{operation}", SpanKind.Client);
            span.SetAttribute("db.system", "sql");
            span.SetAttribute("db.operation", typeof(TReq).Name);

            try
            {
                return await dbOperation();
            }
            catch (Exception ex)
            {
                HandleDbException(ex, span, req);
                throw;
            }
            finally
            {
                span.End();
            }
        }

        #endregion

        #region Exception & Logging Helpers

        private void HandleDbException<TReq>(Exception ex, TelemetrySpan span, TReq req)
        {
            span.SetAttribute("status", "failure");
            span.RecordException(ex);

            switch (ex)
            {
                case DbUpdateException dbEx:
                    LogDbUpdateException(dbEx, span);
                    throw new DBExecutionException<TReq>("Database operation failed", dbEx, req);

                case SqlException sqlEx:
                    LogSqlException(sqlEx, span);
                    throw new DBExecutionException<TReq>("SQL error occurred", sqlEx, req);

                default:
                    _logger.LogError(ex, "Unexpected error occurred");
                    throw new DBExecutionException<TReq>("An unexpected error occurred", ex, req);
            }
        }

        private void LogDbUpdateException(DbUpdateException dbEx, TelemetrySpan span)
        {
            var entityNames = string.Join(",", dbEx.Entries.Select(e => e.GetType().Name));
            span.SetAttribute("db.failed_entities", entityNames);
            _logger.LogError(dbEx, "Database operation failed");
        }

        private void LogSqlException(SqlException sqlEx, TelemetrySpan span)
        {
            span.SetAttribute("sql.error.number", sqlEx.Number);
            span.SetAttribute("sql.error.line", sqlEx.LineNumber);
            span.SetAttribute("sql.error.procedure", sqlEx.Procedure ?? "N/A");
            _logger.LogError(sqlEx, "SQL error occurred");
        }

        private string GetCaller() => new StackTrace().GetFrame(2)?.GetMethod()?.Name ?? "Unknown";

        #endregion
    }
}
