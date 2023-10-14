using System.Linq;
using SimWinInput;

namespace Key2Joy.LowLevelInput;

public class GamePadManager
{
    public const int MAX_GAMEPADS = 4;

    private static GamePadManager instance;
    public static GamePadManager Instance
    {
        get
        {
            instance ??= new GamePadManager();

            return instance;
        }
    }

    private readonly bool[] pluggedInGamePads = new bool[MAX_GAMEPADS];

    private GamePadManager() { }

    public void EnsurePluggedIn(int gamePadIndex)
    {
        if (this.pluggedInGamePads[gamePadIndex])
        {
            return;
        }

        SimGamePad.Instance.PlugIn(gamePadIndex);
        this.pluggedInGamePads[gamePadIndex] = true;
    }

    public int[] GetAllGamePadIndices() => Enumerable.Range(0, MAX_GAMEPADS).ToArray();

    public void EnsureUnplugged(int gamePadIndex)
    {
        if (!this.pluggedInGamePads[gamePadIndex])
        {
            return;
        }

        SimGamePad.Instance.Unplug(gamePadIndex);
        this.pluggedInGamePads[gamePadIndex] = false;
    }

    public void EnsureAllUnplugged()
    {
        for (var i = 0; i < MAX_GAMEPADS; i++)
        {
            this.EnsureUnplugged(i);
        }
    }
}
