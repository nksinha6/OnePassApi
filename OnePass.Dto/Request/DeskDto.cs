using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Dto
{
    public class DeskDto
    {
        public string Name { get; set; }
        public Guid UnitId { get; set; }
        public string? AdminPhone { get; set; }
        public Guid? AccessModeId { get; set; }
        public Guid? AccessCategoryId { get; set; }
    }

}
