using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain.Services
{
    public class PremisePersistService(IPersistRepository<Premise> premiseRepository, IPersistRepository<Property> propertyRepository,
        IPersistRepository<Unit> unitRepository,
        IPersistRepository<Desk> deskRepository) : IPremisePersistService
    {
        private readonly IPersistRepository<Premise> _premiseRepository = premiseRepository;
        private readonly IPersistRepository<Property> _propertyRepository = propertyRepository;
        private readonly IPersistRepository<Unit> _unitRepository = unitRepository;
        private readonly IPersistRepository<Desk> _deskRepository = deskRepository;

        public async Task<Premise> PersistPremise(Premise premise)
        {
            return (await _premiseRepository.AddOrUpdateAllAsync(new List<Premise>() { premise })).FirstOrDefault();
        }

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

        public async Task UpdatePremise(Premise premise)
        {
            await _premiseRepository.UpdateAsync(premise);
        }

        public async Task DeletePremise(Premise premise)
        {
            await _premiseRepository.DeleteAsync(premise);
        }

        public async Task UpdatePremisePartial(Premise premise, params Expression<Func<Premise, object>>[] properties)
        {
            await _premiseRepository.UpdatePartialAsync(premise, properties);
        }
    }
}
