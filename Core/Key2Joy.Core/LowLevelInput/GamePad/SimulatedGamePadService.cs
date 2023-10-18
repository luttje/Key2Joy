using System;
using System.Linq;
using SimWinInput;

namespace Key2Joy.LowLevelInput.GamePad;

public class SimulatedGamePadService : IGamePadService
{
    private const int MAX_GAMEPADS = 4;
    private readonly IGamePad[] gamePads;

    public SimulatedGamePadService(IGamePad[] gamePads = null)
    {
        if (gamePads != null)
        {
            this.gamePads = gamePads;
            return;
        }

        this.gamePads = new IGamePad[MAX_GAMEPADS];

        for (var index = 0; index < MAX_GAMEPADS; index++)
        {
            this.gamePads[index] = new SimulatedGamePad(index);
        }
    }

    public void Initialize() => SimGamePad.Instance.Initialize();

    public void ShutDown() => SimGamePad.Instance.ShutDown();

    public IGamePad GetGamePad(int gamePadIndex)
        => this.gamePads[gamePadIndex];

    public IGamePad[] GetAllGamePads()
        => this.gamePads;

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

    public void EnsureAllUnplugged()
    {
        foreach (var gamePad in this.gamePads.Where(gamePad => gamePad.GetIsPluggedIn()))
        {
            gamePad.Unplug();
        }
    }
}
