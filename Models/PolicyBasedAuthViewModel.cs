using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PBA.Models
{
    public class PolicyBasedAuthViewModel
    {
        public bool IsCris { get; set; }
        
        public bool IsCrisEmail { get; set; }

        public bool IsOverAge { get; set; }
    }
}
