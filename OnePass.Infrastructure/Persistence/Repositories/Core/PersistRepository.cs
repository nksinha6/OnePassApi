using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnePass.Domain;
using System.Linq.Expressions;
using Microsoft.Data.SqlClient;
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

        public async Task<T> AddAsync(T entity)
        {
            await ExecuteDbOperation(entity, async () =>
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
            });

            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            await ExecuteDbOperation(entity, async () =>
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
            });
        }

        public async Task DeleteAsync(T entity)
        {
            await ExecuteDbOperation(entity, async () =>
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            });
        }

        public async Task<IEnumerable<T>> AddAllAsync(IEnumerable<T> entities)
        {
            await ExecuteDbOperation(entities, async () =>
            {
                if (entities == null || !entities.Any())
                {
                    _logger.LogInformation("AddAllAsync called with empty or null collection for type {EntityType}", typeof(T).Name);
                    return;
                }

                await _dbSet.AddRangeAsync(entities);
                await _context.SaveChangesAsync();
            });

            return entities;
        }

        public async Task UpdateAllAsync(IEnumerable<T> entities)
        {
            await ExecuteDbOperation(entities, async () =>
            {
                if (entities == null || !entities.Any())
                {
                    _logger.LogInformation("UpdateAllAsync called with empty or null collection for type {EntityType}", typeof(T).Name);
                    return;
                }

                _dbSet.UpdateRange(entities);
                await _context.SaveChangesAsync();
            });
        }

        public async Task DeleteAllAsync(IEnumerable<T> entities)
        {
            await ExecuteDbOperation(entities, async () =>
            {
                if (entities == null || !entities.Any())
                {
                    _logger.LogInformation("DeleteAllAsync called with empty or null collection for type {EntityType}", typeof(T).Name);
                    return;
                }

                _dbSet.RemoveRange(entities);
                await _context.SaveChangesAsync();
            });
        }

        public async Task<IEnumerable<T>> AddOrUpdateAllAsync(IEnumerable<T> entities)
        {
            await ExecuteDbOperation(entities, async () =>
            {
                if (entities == null || !entities.Any())
                {
                    _logger.LogInformation("AddOrUpdateAllAsync called with empty or null collection for type {EntityType}", typeof(T).Name);
                    return;
                }

                foreach (var entity in entities)
                {
                    var entry = _context.Entry(entity);
                    if (entry.State == EntityState.Detached)
                    {
                        var key = _context.Model.FindEntityType(typeof(T))?.FindPrimaryKey();
                        var keyValues = key?.Properties.Select(p => p.PropertyInfo.GetValue(entity)).ToArray();
                        var existing = await _dbSet.FindAsync(keyValues);

                        if (existing != null)
                        {
                            _context.Entry(existing).CurrentValues.SetValues(entity);

                            var createdAtProp = typeof(T).GetProperty("CreatedAt");
                            if (createdAtProp != null && createdAtProp.CanWrite)
                            {
                                var value = createdAtProp.GetValue(existing);
                                createdAtProp.SetValue(existing, value ?? DateTime.UtcNow);
                            }
                        }
                        else
                        {
                            var createdAtProp = typeof(T).GetProperty("CreatedAt");
                            if (createdAtProp != null && createdAtProp.CanWrite)
                            {
                                var currentValue = createdAtProp.GetValue(entity);
                                if (currentValue == null || (currentValue is DateTime dt && dt == default))
                                {
                                    createdAtProp.SetValue(entity, DateTime.UtcNow);
                                }
                            }

                            await _dbSet.AddAsync(entity);
                        }
                    }
                }

                await _context.SaveChangesAsync();
            });

            return entities;
        }

        public async Task UpdatePartialAsync(T entity, params Expression<Func<T, object>>[] updatedProperties)
        {
            await ExecuteDbOperation(entity, async () =>
            {
                _dbSet.Attach(entity);
                var entry = _context.Entry(entity);

                foreach (var property in updatedProperties)
                {
                    entry.Property(property).IsModified = true;
                }

                await _context.SaveChangesAsync();
            });
        }

        private async Task ExecuteDbOperation<TReq>(TReq req, Func<Task> dbOperation)
        {
            var operation = new StackTrace().GetFrame(1)?.GetMethod()?.Name ?? "Unknown";
            var span = _tracer.StartSpan($"DB:{operation}", SpanKind.Client);
            span.SetAttribute("db.system", "sql");
            span.SetAttribute("db.operation", typeof(TReq).Name);
            try
            {
                await dbOperation();
            }
            catch (DbUpdateException dbEx)
            {
                span.SetAttribute("status", "failure");
                var entityNames = string.Join(",", dbEx.Entries.Select(e => e.GetType().Name));
                span.SetAttribute("db.failed_entities", entityNames);
                span.RecordException(dbEx);
                _logger.LogError(dbEx, "Database operation failed");

                throw new DBExecutionException<TReq>("Database operation failed", dbEx, req);
            }
            catch (SqlException sqlEx)
            {
                span.SetAttribute("status", "failure");
                _logger.LogError(sqlEx, "SQL error occurred");
                span.SetAttribute("sql.error.number", sqlEx.Number);
                span.SetAttribute("sql.error.line", sqlEx.LineNumber);
                span.SetAttribute("sql.error.procedure", sqlEx.Procedure ?? "N/A");
                span.RecordException(sqlEx);

                throw new DBExecutionException<TReq>("SQL error occurred", sqlEx, req);
            }
            catch (Exception ex)
            {
                span.SetAttribute("status", "failure");
                span.RecordException(ex);
                _logger.LogError(ex, "Unexpected error occurred");

                throw new DBExecutionException<TReq>("An unexpected error occurred", ex, req);
            }
            finally
            {
                span.End();
            }
        }

    }

}
