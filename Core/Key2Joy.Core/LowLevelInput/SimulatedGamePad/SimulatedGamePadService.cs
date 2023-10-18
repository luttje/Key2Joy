using System;
using System.Linq;
using SimWinInput;

namespace Key2Joy.LowLevelInput.SimulatedGamePad;

/// <inheritdoc />
public class SimulatedGamePadService : ISimulatedGamePadService
{
    private const int MAX_GAMEPADS = 4;
    private readonly ISimulatedGamePad[] gamePads;

    public SimulatedGamePadService(ISimulatedGamePad[] gamePads = null)
    {
        if (gamePads != null)
        {
            this.gamePads = gamePads;
            return;
        }

        this.gamePads = new ISimulatedGamePad[MAX_GAMEPADS];

        for (var index = 0; index < MAX_GAMEPADS; index++)
        {
            this.gamePads[index] = new SimulatedGamePad(index);
        }
    }

    /// <inheritdoc />
    public void Initialize() => SimGamePad.Instance.Initialize();

    /// <inheritdoc />
    public void ShutDown() => SimGamePad.Instance.ShutDown();

    /// <inheritdoc />
    public ISimulatedGamePad GetGamePad(int gamePadIndex)
        => this.gamePads[gamePadIndex];

    /// <inheritdoc />
    public ISimulatedGamePad[] GetAllGamePads()
        => this.gamePads;

    /// <inheritdoc />
    public void EnsurePluggedIn(int gamePadIndex)
    {
        if (gamePadIndex is < 0 or >= MAX_GAMEPADS)
        {
            throw new ArgumentOutOfRangeException(nameof(gamePadIndex));
        }

        var gamePad = this.gamePads[gamePadIndex];

        if (gamePad.GetIsPluggedIn())
        {
            return;
        }

        gamePad.PlugIn();
    }

    /// <inheritdoc />
    public void EnsureUnplugged(int gamePadIndex)
    {
        if (gamePadIndex is < 0 or >= MAX_GAMEPADS)
        {
            throw new ArgumentOutOfRangeException(nameof(gamePadIndex));
        }

        var gamePad = this.gamePads[gamePadIndex];

        if (!gamePad.GetIsPluggedIn())
        {
            return;
        }

        gamePad.Unplug();
    }

    /// <inheritdoc />
    public void EnsureAllUnplugged()
    {
        foreach (var gamePad in this.gamePads.Where(gamePad => gamePad.GetIsPluggedIn()))
        {
            gamePad.Unplug();
        }
    }
}
