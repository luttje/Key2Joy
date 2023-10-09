namespace Key2Joy.Mapping.Actions.Logic;

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
}
