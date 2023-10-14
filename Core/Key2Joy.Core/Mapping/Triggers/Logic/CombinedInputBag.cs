using System.Collections.Generic;
using Key2Joy.Contracts.Mapping.Triggers;

namespace Key2Joy.Mapping.Triggers.Logic;

public class CombinedInputBag : AbstractInputBag
{
    public List<AbstractInputBag> InputBags;
}
