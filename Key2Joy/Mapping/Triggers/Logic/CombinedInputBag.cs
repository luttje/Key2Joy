using Key2Joy.Contracts.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    public class CombinedInputBag : AbstractInputBag
    {
        public List<AbstractInputBag> InputBags;
    }
}
