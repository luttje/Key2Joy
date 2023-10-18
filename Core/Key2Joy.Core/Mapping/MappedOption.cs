using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.LowLevelInput;

namespace Key2Joy.Mapping;

public class MappedOption : AbstractMappedOption
{
    [JsonIgnore]
    public bool IsChild => this.ParentGuid != null;

    [JsonIgnore]
    public MappedOption Parent { get; set; }

    [JsonIgnore]
    public IList<MappedOption> Children { get; set; } = new List<MappedOption>();

    public MappedOption()
        : base()
        => this.Guid = Guid.NewGuid();

    public MappedOption(Guid guid)
        : base()
        => this.Guid = guid;

    public void SetParent(MappedOption parent)
    {
        if (parent == null)
        {
            this.Parent.Children.Remove(this);
            this.ParentGuid = null;
            this.Parent = null;

            return;
        }

        this.ParentGuid = parent.Guid;
        this.Parent = parent;
        parent.Children.Add(this);
    }

    public bool IsChildOf(MappedOption parent)
        => this.ParentGuid != null && this.ParentGuid == parent.Guid;

    public void Initialize(IList<MappedOption> allMappedOptions)
    {
        this.Children = new List<MappedOption>();

        foreach (var mappedOption in allMappedOptions)
        {
            if (mappedOption.ParentGuid.Equals(this.Guid))
            {
                this.Children.Add(mappedOption);
                mappedOption.Parent = this;
            }
        }
    }

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
