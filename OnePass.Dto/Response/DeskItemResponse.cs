using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Dto
{
    public class DeskItemResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? AdminPhone { get; set; }
        public Guid? AccessModeId { get; set; }
        public string? AccessMode { get; set; }
        public Guid? AccessCategoryId { get; set; }
        public string? AccessCategory { get; set; }
    }
}
