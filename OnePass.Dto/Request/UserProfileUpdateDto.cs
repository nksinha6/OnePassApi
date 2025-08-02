using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Dto
{
    public class UserProfileUpdateDto
    {
        public string Phone { get; set; } = null!;   // PK
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string? Email
        {
            get; set;
        }
    }
}
