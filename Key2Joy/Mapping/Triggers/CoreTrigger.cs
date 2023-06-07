using Key2Joy.Contracts.Mapping;
using System;
using System.Drawing;

namespace Key2Joy.Mapping
{
    public abstract class CoreTrigger : AbstractTrigger
    {
        public string ImageResource { get; set; }

        public CoreTrigger(string name)
            : base(name)
        {
        }
    }
}
