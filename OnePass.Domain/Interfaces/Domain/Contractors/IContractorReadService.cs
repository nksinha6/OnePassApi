using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public interface IContractorReadService
    {
        Task<IEnumerable<ContractorSelfieImageResponse>> GetAllContractorSelfieImagesAsync();

        public Task<(string, string?, string?)> CompareSelfie(ImageInput input);
    }
}
