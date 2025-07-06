using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public interface IStoredProcReadRepository<TQuery, TResult> : IReadRepository<TQuery, TResult>
        where TResult : class, new()
        where TQuery : IReadQuery
    {
    }
}
