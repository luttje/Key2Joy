namespace Key2Joy.LowLevelInput;

public enum PressState
{
    /// <summary>
    /// The key/button is pressed down
    /// </summary>
    Press,

    /// <summary>
    /// The key/button is released (after having been pressed down)
    /// </summary>
    Release,
}

public static class PressStates
{
    /// <summary>
    /// All available press states
    /// </summary>
    public static readonly PressState[] ALL = new PressState[] {
        PressState.Press,
        PressState.Release
    };
}
