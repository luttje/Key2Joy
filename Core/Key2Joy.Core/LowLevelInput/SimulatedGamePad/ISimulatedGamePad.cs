using SimWinInput;

namespace Key2Joy.LowLevelInput.SimulatedGamePad;

/// <summary>
/// Represents a simulated gamepad device.
/// </summary>
public interface ISimulatedGamePad
{
    /// <summary>
    /// Gets the index of the simulated gamepad.
    /// </summary>
    int Index { get; }

    /// <summary>
    /// Plugs in the simulated gamepad.
    /// </summary>
    void PlugIn();

    /// <summary>
    /// Checks if the simulated gamepad is currently plugged in.
    /// </summary>
    /// <returns>True if the gamepad is plugged in; otherwise, false.</returns>
    bool GetIsPluggedIn();

    /// <summary>
    /// Unplugs the simulated gamepad.
    /// </summary>
    void Unplug();

    /// <summary>
    /// Simulates pressing and holding a control on the gamepad.
    /// </summary>
    /// <param name="control">The control to simulate.</param>
    /// <param name="holdTimeMS">The duration (in milliseconds) to hold the control (default is 50ms).</param>
    void Use(GamePadControl control, int holdTimeMS = 50);

    /// <summary>
    /// Sets a specific control on the gamepad.
    /// </summary>
    /// <param name="control">The control to set.</param>
    void SetControl(GamePadControl control);

    /// <summary>
    /// Releases a specific control on the gamepad.
    /// </summary>
    /// <param name="control">The control to release.</param>
    void ReleaseControl(GamePadControl control);

    /// <summary>
    /// Get the raw input state from the GamePad
    /// </summary>
    /// <returns>The raw input state of the gamepad.</returns>
    SimulatedGamePadState GetState();

    /// <summary>
    /// Resets the GamePad state to the natural at-rest stat
    /// </summary>
    void ResetState();

    /// <summary>
    /// Update any changes made to the state to be reflected in the gamepad
    /// </summary>
    void Update();
}
