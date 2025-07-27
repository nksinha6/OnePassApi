using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain.Services
{
    public class PremisePersistService(IPersistRepository<Property> propertyRepository,
        IPersistRepository<Unit> unitRepository,
        IPersistRepository<Desk> deskRepository) : IPremisePersistService
    {
       private readonly IPersistRepository<Property> _propertyRepository = propertyRepository;
        private readonly IPersistRepository<Unit> _unitRepository = unitRepository;
        private readonly IPersistRepository<Desk> _deskRepository = deskRepository;

        
        public async Task<Property> PersistProperty(Property property)
        {
            return (await _propertyRepository.AddOrUpdateAllAsync(new List<Property>() { property })).FirstOrDefault();
        }

        public async Task<Unit> PersistUnit(Unit unit)
        {
            return (await _unitRepository.AddOrUpdateAllAsync(new List<Unit>() { unit })).FirstOrDefault();
        }

        public async Task<Desk> PersistDesk(Desk desk)
        {
            return (await _deskRepository.AddOrUpdateAllAsync(new List<Desk>() { desk })).FirstOrDefault();
        }

        public async Task UpdatePremisePartial(Property premise, params Expression<Func<Property, object>>[] properties)
        {
            await _propertyRepository.UpdatePartialAsync(premise, properties);
        }
    }
}
