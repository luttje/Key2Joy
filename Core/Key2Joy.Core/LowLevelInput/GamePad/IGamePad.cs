using SimWinInput;

namespace Key2Joy.LowLevelInput.GamePad;

public interface IGamePad
{
    int Index { get; set; }

    void PlugIn();

    bool GetIsPluggedIn();

    void Unplug();

    void Use(GamePadControl control, int holdTimeMS = 50);

    void SetControl(GamePadControl control);

    void ReleaseControl(GamePadControl control);

    /// <summary>
    /// Get the raw input state from the GamePad
    /// </summary>
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
