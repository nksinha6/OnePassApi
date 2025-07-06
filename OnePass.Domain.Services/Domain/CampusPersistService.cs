using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain.Services
{
    public class CampusPersistService : ICampusPersistService
    {
        private readonly IPersistRepository<Campus> _campusRepository;

        public CampusPersistService(IPersistRepository<Campus> campusRepository)
        {
            _campusRepository = campusRepository;
        }

        public async Task PersistCampus(Campus campus)
        {
            await _campusRepository.AddAsync(campus);
        }

        public async Task UpdateCampus(Campus campus)
        {
            await _campusRepository.UpdateAsync(campus);
        }

        public async Task DeleteCampus(Campus campus)
        {
            await _campusRepository.DeleteAsync(campus);
        }

        public async Task UpdateCampusPartial(Campus campus, params Expression<Func<Campus, object>>[] properties)
        {
            await _campusRepository.UpdatePartialAsync(campus, properties);
        }
    }
}
