using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TD02.Models
{
    public class Capital
    {
        public int toll { get; set; }
        public Dictionary<string, int> neighbors { get; set; }
    }
}