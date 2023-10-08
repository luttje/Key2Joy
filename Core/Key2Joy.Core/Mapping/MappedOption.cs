using Key2Joy.Contracts.Mapping;

namespace Key2Joy.Mapping
{
    public class MappedOption : AbstractMappedOption
    {
        public override object Clone()
        {
            return new MappedOption()
            {
                Trigger = Trigger != null ? (AbstractTrigger)Trigger.Clone() : null,
                Action = (AbstractAction)Action.Clone(),
            };
        }
    }
}
