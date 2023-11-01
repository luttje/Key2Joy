namespace Key2Joy.Mapping.Actions.Logic;

/// <summary>
/// The possible commands to run in the app.
/// </summary>
public enum AppCommand : int
{
    /// <summary>
    /// Aborts listening for triggers
    /// </summary>
    Abort = 0,

    /// <summary>
    /// Recreate the scripting environment (loses all variables, functions and other changes scripts made)
    /// </summary>
    ResetScriptEnvironment = 10,

    /// <summary>
    /// Can be called to reset the cursor to the center. Useful for games that take control of the cursor
    /// and place it somewhere else.
    /// </summary>
    ResetMouseMoveTriggerCenter = 20,
}
