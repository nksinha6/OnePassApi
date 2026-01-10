using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public sealed record ImageInput(
    Stream Stream,
    string FileName,
    string ContentType,
    long Length
);

}
