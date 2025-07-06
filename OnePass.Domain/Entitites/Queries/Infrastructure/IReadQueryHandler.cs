using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public interface IReadQueryHandler<in TQuery, TResult>
       where TQuery : IReadQuery
       where TResult : class
    {
        Task<IEnumerable<TResult>> HandleAllAsync();
        Task<IEnumerable<TResult>> HandleQueryAsync(TQuery query);
    }
}
