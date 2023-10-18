using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;

namespace Key2Joy.Contracts.Mapping;

public abstract class AbstractMappedOption : ICloneable
{
    /// <summary>
    /// The unique identifier for this mapping.
    /// This way it can be referenced by other mappings.
    /// </summary>
    [JsonInclude]
    public Guid Guid { get; protected set; }

    /// <summary>
    /// The action that is executed when this mapping is executed.
    /// </summary>
    [JsonInclude]
    public AbstractAction Action { get; set; }

    /// <summary>
    /// The trigger that causes this mapping to be executed.
    /// </summary>
    [JsonInclude]
    public AbstractTrigger Trigger { get; set; }

    /// <summary>
    /// The unique identifier of the parent mapped option.
    /// </summary>
    [JsonInclude]
    public Guid? ParentGuid { get; set; } = null;

    [JsonIgnore]
    public bool IsChild => this.ParentGuid != null;

    [JsonIgnore]
    public AbstractMappedOption Parent { get; set; }

    [JsonIgnore]
    public IList<AbstractMappedOption> Children { get; set; } = new List<AbstractMappedOption>();

    public void SetParent(AbstractMappedOption parent)
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

    public bool IsChildOf(AbstractMappedOption parent)
        => this.ParentGuid != null && this.ParentGuid == parent.Guid;

    public void Initialize(IEnumerable<AbstractMappedOption> allMappedOptions)
    {
        this.Children = new List<AbstractMappedOption>();

        foreach (var mappedOption in allMappedOptions)
        {
            if (mappedOption.ParentGuid.Equals(this.Guid))
            {
                this.Children.Add(mappedOption);
                mappedOption.Parent = this;
            }
        }
    }

    public abstract object Clone();
}
