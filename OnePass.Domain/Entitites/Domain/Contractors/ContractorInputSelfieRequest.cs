using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace OnePass.Domain
{
    public class ContractorInputSelfieRequest
    {
        public IFormFile Selfie { get; set; } = null!;

    }
}
