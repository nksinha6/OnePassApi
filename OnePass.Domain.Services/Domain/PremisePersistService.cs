using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain.Services
{
    public class PremisePersistService : IPremisePersistService
    {
        private readonly IPersistRepository<Property> _propertyRepository;
        private readonly IPersistRepository<Unit> _unitRepository;
        private readonly IPersistRepository<Desk> _deskRepository;

        public PremisePersistService(
            IPersistRepository<Property> propertyRepository,
            IPersistRepository<Unit> unitRepository,
            IPersistRepository<Desk> deskRepository)
        {
            _propertyRepository = propertyRepository;
            _unitRepository = unitRepository;
            _deskRepository = deskRepository;
        }

        // ✅ DRY helper method
        private static async Task<T> PersistSingleAsync<T>(IPersistRepository<T> repository, T entity) where T : class
        {
            var result = await repository.AddOrUpdateAllAsync(new List<T> { entity });
            return result.First();
        }

        public Task<Property> PersistProperty(Property property) =>
            PersistSingleAsync(_propertyRepository, property);

        public Task<Unit> PersistUnit(Unit unit) =>
            PersistSingleAsync(_unitRepository, unit);

        public Task<Desk> PersistDesk(Desk desk) =>
            PersistSingleAsync(_deskRepository, desk);

        public Task UpdatePremisePartial(Property premise, params Expression<Func<Property, object>>[] properties) =>
            _propertyRepository.UpdatePartialAsync(premise, properties);
    }
}
