using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    internal class CombinedInputBag : IInputBag
    {
        public List<IInputBag> InputBags;
    }
}
