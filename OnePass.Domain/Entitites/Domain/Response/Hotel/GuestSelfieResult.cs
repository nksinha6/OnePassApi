using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
     public class GuestSelfieResult
    {
        public Stream Stream { get; init; } = default!;
        public string ContentType { get; init; } = default!;
        public long FileSize { get; init; }
    }
}
