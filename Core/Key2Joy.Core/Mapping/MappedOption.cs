using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;

namespace Key2Joy.Mapping
{
    public class MappedOption : AbstractMappedOption
    {
        public override object Clone()
        {
            return new MappedOption()
            {
                Trigger = this.Trigger != null ? (AbstractTrigger)this.Trigger.Clone() : null,
                Action = (AbstractAction)this.Action.Clone(),
            };
        }
    }
}
