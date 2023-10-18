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

    public abstract object Clone();
}
