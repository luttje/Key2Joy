using Key2Joy.Contracts.Mapping;

namespace Key2Joy.Mapping
{
    public class MappedOption : AbstractMappedOption
    {
        public string GetActionDisplay()
        {
            return Action.GetNameDisplay();
        }

        public object GetTriggerDisplay()
        {
            if (Trigger == null)
                return "<not bound to key>";

            return Trigger.ToString();
        }

        public override object Clone()
        {
            return new MappedOption()
            {
                Trigger = Trigger != null ? (CoreTrigger)Trigger.Clone() : null,
                Action = (CoreAction)Action.Clone(),
            };
        }
    }
}
