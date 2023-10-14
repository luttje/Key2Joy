using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Triggers;

namespace Key2Joy.Mapping.Triggers;

public class DisabledTriggerListener : CoreTriggerListener
{
    private static DisabledTriggerListener instance;

    public static DisabledTriggerListener Instance
    {
        get
        {
            instance ??= new DisabledTriggerListener();

            return instance;
        }
    }

    protected DisabledTriggerListener()
    { }

    public override void AddMappedOption(AbstractMappedOption mappedOption)
    { }

    public override bool GetIsTriggered(AbstractTrigger trigger) => false;
}
