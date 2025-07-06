using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public interface IPersistRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);

        Task AddAllAsync(IEnumerable<T> entities);
        Task UpdateAllAsync(IEnumerable<T> entities);
        Task DeleteAllAsync(IEnumerable<T> entities);

        Task AddOrUpdateAllAsync(IEnumerable<T> entities);

        Task UpdatePartialAsync(T entity, params Expression<Func<T, object>>[] updatedProperties);
    }
}
