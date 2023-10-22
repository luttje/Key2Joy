using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;

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

    public override string ToString()
        => $"[{this.Guid}] Trigger: {this.Trigger} -> Action: {this.Action}";

    /// <inheritdoc/>
    public override object Clone() => new MappedOption(this.Guid)
    {
        Trigger = this.Trigger != null ? (AbstractTrigger)this.Trigger.Clone() : null,
        Action = (AbstractAction)this.Action.Clone(),
        ParentGuid = this.ParentGuid,
    };

    /// <summary>
    /// Goes through all provided mappings and asks them to provide the reverse for their
    /// action and trigger. If no <see cref="IProvideReverseAspect"/> is implemented, a
    /// copy of the current mapping is returned.
    /// </summary>
    /// <param name="mappings"></param>
    /// <returns></returns>
    public static List<MappedOption> GenerateReverseMappings(List<MappedOption> mappings)
    {
        List<MappedOption> newOptions = new();

        foreach (var mapping in mappings)
        {
            newOptions.Add(GenerateReverseMapping(mapping));
        }

        return newOptions;
    }

    /// <summary>
    /// Asks the provided mappings for a variant with reverse action and trigger.
    /// If no <see cref="IProvideReverseAspect"/> is implemented, a copy of the
    /// current mapping is returned.
    /// </summary>
    /// <param name="mapping"></param>
    /// <param name="dontSetParent">Optionally dont set the parent, useful to get a reverse that wont be saved.</param>
    /// <returns></returns>
    public static MappedOption GenerateReverseMapping(MappedOption mapping, bool dontSetParent = false)
    {
        var actionCopy = (AbstractAction)mapping.Action.Clone();
        var triggerCopy = (AbstractTrigger)mapping.Trigger.Clone();

        if (mapping.Action is IProvideReverseAspect action)
        {
            action.MakeReverse(actionCopy);
        }

        if (mapping.Trigger is IProvideReverseAspect trigger)
        {
            trigger.MakeReverse(triggerCopy);
        }

        MappedOption variantOption = new()
        {
            Action = actionCopy,
            Trigger = triggerCopy,
        };

        if (!dontSetParent)
        {
            variantOption.SetParent(mapping);
        }

        return variantOption;
    }
}
