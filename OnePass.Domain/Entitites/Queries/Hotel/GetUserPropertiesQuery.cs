using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public sealed class GetUserPropertiesQuery : IReadQuery
    {
        public string UserId { get; set; } 
    }

}
