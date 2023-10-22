namespace Key2Joy.Config;

/// <summary>
/// The type of grouping when listing mapped options in the UI
/// </summary>
public enum ViewMappingGroupType
{
    /// <summary>
    /// No grouping
    /// </summary>
    None,

    /// <summary>
    /// Group by action GroupName's
    /// </summary>
    ByAction,

    /// <summary>
    /// Group by trigger GroupName's
    /// </summary>
    ByTrigger
}
