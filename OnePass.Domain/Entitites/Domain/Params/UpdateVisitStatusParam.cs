﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class UpdateVisitStatusParam
    {
        public Guid VisitId { get; set; }
        public string Status { get; set; }
    }
}
