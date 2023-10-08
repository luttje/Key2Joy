using SimWinInput;
using System.Linq;

namespace Key2Joy.LowLevelInput
{
    public class GamePadManager
    {
        public const int MAX_GAMEPADS = 4;

        private static GamePadManager instance;
        public static GamePadManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new GamePadManager();

                return instance;
            }
        }

        private bool[] pluggedInGamePads = new bool[MAX_GAMEPADS];

        private GamePadManager() { }

        public void EnsurePluggedIn(int gamePadIndex)
        {
            if (pluggedInGamePads[gamePadIndex])
                return;

            SimGamePad.Instance.PlugIn(gamePadIndex);
            pluggedInGamePads[gamePadIndex] = true;
        }

        public int[] GetAllGamePadIndices()
        {
            return Enumerable.Range(0, MAX_GAMEPADS).ToArray();
        }

        public void EnsureUnplugged(int gamePadIndex)
        {
            if (!pluggedInGamePads[gamePadIndex])
                return;

            SimGamePad.Instance.Unplug(gamePadIndex);
            pluggedInGamePads[gamePadIndex] = false;
        }

        public void EnsureAllUnplugged()
        {
            for (int i = 0; i < MAX_GAMEPADS; i++)
                EnsureUnplugged(i);
        }
    }
}
