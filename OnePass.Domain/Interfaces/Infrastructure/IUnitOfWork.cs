using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Get the generic repository for an entity.
        /// </summary>
        IPersistRepository<T> Repository<T>() where T : class;

        /// <summary>
        /// Executes the given operation inside a single database transaction.
        /// Commits if successful, rolls back on failure.
        /// </summary>
        Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> operation);

        /// <summary>
        /// Saves all pending changes to the database (if not already done inside ExecuteInTransactionAsync).
        /// </summary>
        Task<int> CompleteAsync();
    }

}
