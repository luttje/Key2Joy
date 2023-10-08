using Key2Joy.Contracts.Mapping;
using System.Collections.Generic;

namespace Key2Joy.Mapping
{
    public class CombinedInputBag : AbstractInputBag
    {
        public List<AbstractInputBag> InputBags;
    }
}
