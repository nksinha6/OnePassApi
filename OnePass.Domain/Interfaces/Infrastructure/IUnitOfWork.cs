using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        IPersistRepository<T> Repository<T>() where T : class;

        Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> operation);

        Task<int> CompleteAsync();
    }
}
