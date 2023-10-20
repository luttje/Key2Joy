using SimWinInput;

namespace Key2Joy.LowLevelInput.SimulatedGamePad;

/// <summary>
/// Implementation based on https://github.com/DavidRieman/SimWinInput
/// </summary>
public class SimulatedGamePad : ISimulatedGamePad
{
    /// <inheritdoc />
    public int Index { get; private set; }

    private bool isPluggedIn = false;

    public SimulatedGamePad(int index)
        => this.Index = index;

    /// <inheritdoc />
    public void PlugIn()
    {
        SimGamePad.Instance.PlugIn(this.Index);
        this.isPluggedIn = true;

        // Ensure the state starts reset fixes problem where other (real) gamepad may get button stuck
        this.ResetState();
        this.Update();
    }

    /// <inheritdoc />
    public bool GetIsPluggedIn()
        => this.isPluggedIn;

    /// <inheritdoc />
    public void Unplug()
    {
        SimGamePad.Instance.Unplug(this.Index);
        this.isPluggedIn = false;
    }

    /// <inheritdoc />
    public void Use(GamePadControl control, int holdTimeMS = 50)
        => SimGamePad.Instance.Use(control, this.Index, holdTimeMS);

    /// <inheritdoc />
    public void SetControl(GamePadControl control)
        => SimGamePad.Instance.SetControl(control, this.Index);

    /// <inheritdoc />
    public void ReleaseControl(GamePadControl control)
        => SimGamePad.Instance.ReleaseControl(control, this.Index);

    /// <inheritdoc />
    public SimulatedGamePadState GetState()
        => SimGamePad.Instance.State[this.Index];

    /// <inheritdoc />
    public void ResetState()
        => SimGamePad.Instance.State[this.Index].Reset();

    /// <inheritdoc />
    public void Update()
        => SimGamePad.Instance.Update(this.Index);
}
