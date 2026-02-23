using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Dto
{
    public class HotelPropertyNameResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        
        public PropertyType PropertyType { get; set; }
        public PropertyTier Tier { get; set; }
    }

    public enum PropertyType
    {
        Hospitality,
        Corporate
    }

    public enum PropertyTier
    {
        Starter,
        SMB,
        Enterprise
    }
}
