using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public interface IStoredProcPersistRepository<TCommand> where TCommand : class
    {
        Task<bool> ExecuteCommandAsync(TCommand command);
    }
}
