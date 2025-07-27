using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public interface IPremisePersistService
    {
        Task<Property> PersistProperty(Property property);
        Task<Unit> PersistUnit(Unit unit);
        Task<Desk> PersistDesk(Desk desk);
        Task UpdatePremisePartial(Property premise, params Expression<Func<Property, object>>[] properties);

    }
}
