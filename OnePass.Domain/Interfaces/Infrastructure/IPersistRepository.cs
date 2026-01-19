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
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T> AddOrUpdateAsync(T entity);
        Task<IEnumerable<T>> AddAllAsync(IEnumerable<T> entities);
        Task UpdateAllAsync(IEnumerable<T> entities);
        Task DeleteAllAsync(IEnumerable<T> entities);

        Task<IEnumerable<T>> AddOrUpdateAllAsync(IEnumerable<T> entities);
        Task<T> AddIfNotExistAsync(T entity);
        Task<T> UpdatePartialAsync(T entity, params Expression<Func<T, object>>[] updatedProperties);
    }
}
