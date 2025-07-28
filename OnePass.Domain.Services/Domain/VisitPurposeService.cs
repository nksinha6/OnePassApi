using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain.Services
{
    public class VisitPurposeService(IPersistRepository<VisitPurpose> visitorPersistsRepository) : IVisitPersistService
    {
        private readonly IPersistRepository<VisitPurpose> _visitorPersistsRepository = visitorPersistsRepository;

        public async Task<VisitPurpose> PersistVisitPurposeAsync(VisitPurpose visitPurpose)
        {
            var result = await _visitorPersistsRepository.AddOrUpdateAllAsync(new List<VisitPurpose> { visitPurpose });
            return result.First();
        }
    }
}
