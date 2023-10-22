using System.Collections.Generic;
using Key2Joy.Contracts.Mapping;
using Key2Joy.LowLevelInput;

namespace Key2Joy.Mapping.Triggers;

/// <summary>
/// Base class for trigger listeners that listen for press and release events.
/// </summary>
/// <typeparam name="TTrigger"></typeparam>
public abstract class PressReleaseTriggerListener<TTrigger> : CoreTriggerListener
    where TTrigger : class, IPressState, IReturnInputHash
{
    /// <summary>
    /// Quick lookup by hash for mappings bound to pressing input down
    /// </summary>
    protected Dictionary<int, List<AbstractMappedOption>> LookupPress;

    /// <summary>
    /// Quick lookup by hash for mappings bound to releasing input
    /// </summary>
    protected Dictionary<int, List<AbstractMappedOption>> LookupRelease;

    protected PressReleaseTriggerListener()
    {
        this.LookupPress = new Dictionary<int, List<AbstractMappedOption>>();
        this.LookupRelease = new Dictionary<int, List<AbstractMappedOption>>();
    }

    /// <inheritdoc/>
    public override void AddMappedOption(AbstractMappedOption mappedOption)
    {
        var trigger = mappedOption.Trigger as TTrigger;
        var dictionary = (Dictionary<int, List<AbstractMappedOption>>)null;

        if (trigger.PressState == PressState.Press)
        {
            dictionary = this.LookupPress;
        }

        if (trigger.PressState == PressState.Release)
        {
            dictionary = this.LookupRelease;
        }

        if (dictionary == null)
        {
            return;
        }

        if (!dictionary.TryGetValue(trigger.GetInputHash(), out var mappedOptions))
        {
            dictionary.Add(trigger.GetInputHash(), mappedOptions = new List<AbstractMappedOption>());
        }

        mappedOptions.Add(mappedOption);
    }
}
