using Key2Joy.Contracts.Mapping.Triggers;

namespace Key2Joy.Mapping.Triggers
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
