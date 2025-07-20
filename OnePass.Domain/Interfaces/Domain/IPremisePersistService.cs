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
        Task<Premise> PersistPremise(Premise premise);
        Task<Property> PersistProperty(Property property);
        Task<Unit> PersistUnit(Unit unit);
        Task<Desk> PersistDesk(Desk desk);
        Task UpdatePremise(Premise premise);
        Task DeletePremise(Premise premise);
        Task UpdatePremisePartial(Premise premise, params Expression<Func<Premise, object>>[] properties);

    }
}
