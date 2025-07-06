using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public interface IReadRepository<TQuery, TResult>
       where TResult : class, new()
       where TQuery : IReadQuery
    {
        Task<IEnumerable<TResult>> ExecuteQueryAsync(TQuery query);
        Task<IEnumerable<TResult>> ExecuteAllAsync();
    }
}
