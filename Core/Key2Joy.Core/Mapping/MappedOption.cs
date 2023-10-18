using System;
using System.Collections.Generic;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.LowLevelInput;

namespace Key2Joy.Mapping;

public class MappedOption : AbstractMappedOption
{
    public MappedOption()
        : base()
        => this.Guid = Guid.NewGuid();

    public MappedOption(Guid guid)
        : base()
        => this.Guid = guid;

    public override object Clone() => new MappedOption(this.Guid)
    {
        Trigger = this.Trigger != null ? (AbstractTrigger)this.Trigger.Clone() : null,
        Action = (AbstractAction)this.Action.Clone(),
        ParentGuid = this.ParentGuid,
    };

    public static List<MappedOption> GenerateOppositePressStateMappings(List<MappedOption> mappings)
    {
        List<MappedOption> newOptions = new();

        foreach (var pressVariant in mappings)
        {
            var actionCopy = (AbstractAction)pressVariant.Action.Clone();
            var triggerCopy = (AbstractTrigger)pressVariant.Trigger.Clone();

            if (actionCopy is IPressState actionWithPressState)
            {
                actionWithPressState.PressState = actionWithPressState.PressState == PressState.Press ? PressState.Release : PressState.Press;
            }

            if (triggerCopy is IPressState triggerWithPressState)
            {
                triggerWithPressState.PressState = triggerWithPressState.PressState == PressState.Press ? PressState.Release : PressState.Press;
            }

            MappedOption variantOption = new()
            {
                Action = actionCopy,
                Trigger = triggerCopy,
            };
            variantOption.SetParent(pressVariant);

            newOptions.Add(variantOption);
        }

        return newOptions;
    }
}
