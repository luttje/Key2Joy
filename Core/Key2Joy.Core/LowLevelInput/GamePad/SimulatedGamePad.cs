using SimWinInput;

namespace Key2Joy.LowLevelInput.GamePad;

/// <summary>
/// Implementation based on https://github.com/DavidRieman/SimWinInput
/// </summary>
public class SimulatedGamePad : IGamePad
{
    public int Index { get; private set; }
    private bool isPluggedIn = false;

    public SimulatedGamePad(int index)
        => this.Index = index;

    public void PlugIn()
    {
        this.isPluggedIn = true;
        SimGamePad.Instance.PlugIn(this.Index);
    }

    public bool GetIsPluggedIn()
        => this.isPluggedIn;

    public void Unplug()
    {
        SimGamePad.Instance.Unplug(this.Index);
        this.isPluggedIn = false;
    }

    public void Use(GamePadControl control, int holdTimeMS = 50)
        => SimGamePad.Instance.Use(control, this.Index, holdTimeMS);

    public void SetControl(GamePadControl control)
        => SimGamePad.Instance.SetControl(control, this.Index);

    public void ReleaseControl(GamePadControl control)
        => SimGamePad.Instance.ReleaseControl(control, this.Index);

    public SimulatedGamePadState GetState()
        => SimGamePad.Instance.State[this.Index];

    public void ResetState()
        => SimGamePad.Instance.State[this.Index].Reset();

    public void Update()
        => SimGamePad.Instance.Update(this.Index);
}
