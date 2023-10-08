using Key2Joy.Contracts.Mapping;

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
