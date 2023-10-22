namespace Key2Joy.Mapping.Triggers;

public interface IOverrideDefaultBehavior
{
    /// <summary>
    /// Used by the trigger listener to determine if it should override default behavior.
    /// </summary>
    /// <param name="executedAny"></param>
    /// <returns></returns>
    bool ShouldListenerOverrideDefault(bool executedAny);
}
