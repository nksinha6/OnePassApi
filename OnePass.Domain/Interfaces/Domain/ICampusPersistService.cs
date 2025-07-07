using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public interface ICampusPersistService
    {
        Task<Campus> PersistCampus(Campus campus);
        Task UpdateCampus(Campus campus);
        Task DeleteCampus(Campus campus);
        Task UpdateCampusPartial(Campus campus, params Expression<Func<Campus, object>>[] properties);

    }
}
