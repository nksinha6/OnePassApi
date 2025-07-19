using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OnePass.Domain.Services
{
    public class PremisePersistService(IPersistRepository<Premise> premiseRepository) : IPremisePersistService
    {
        private readonly IPersistRepository<Premise> _premiseRepository = premiseRepository;

        public async Task<Premise> PersistPremise(Premise premise)
        {
            return (await _premiseRepository.AddOrUpdateAllAsync(new List<Premise>() { premise })).FirstOrDefault();
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
