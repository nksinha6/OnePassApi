using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class UserCompanyRole
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid CompanyId { get; set; }

        public Guid UnitId { get; set; }

        public Guid RoleId { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
