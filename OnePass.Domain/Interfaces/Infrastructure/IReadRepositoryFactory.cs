using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public interface IReadRepositoryFactory
    {
        IReadRepository<TQuery, TResult> GetRepository<TQuery, TResult>(bool useStoredProcedure)
            where TResult : class, new()
            where TQuery : IReadQuery;
    }
}
