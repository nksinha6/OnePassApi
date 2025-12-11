using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class SmsRequest
    {

        [Required]
        [RegularExpression(@"^\+?\d{10,15}$")]
        public string To { get; set; }

        // DLT template variables only (var1, var2, etc.)
        [Required]
        public Dictionary<string, string> Variables { get; set; }
    }
}
