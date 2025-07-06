using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnePass.Domain;

namespace OnePass.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OnePassDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(OnePassDbContext context)
        {
            _context = context;
        }

        public IPersistRepository<T> Repository<T>() where T : class
        {
            if (!_repositories.ContainsKey(typeof(T)))
            {
                throw new InvalidOperationException($"No repository found for {typeof(T).Name}");
            }

            return (IPersistRepository<T>)_repositories[typeof(T)];
        }

        public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> operation)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var result = await operation();

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<int> CompleteAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
