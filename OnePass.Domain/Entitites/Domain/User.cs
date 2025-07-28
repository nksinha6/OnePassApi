using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class User
    {
        public string Phone { get; set; } = null!;   // PK

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public string? Email { get; set; }

        public string Status { get; set; } = "unverified";  // allowed values: unverified, created, registered, verified

        public bool IsEmailVerified { get; set; } = false;

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
