using SimWinInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.LowLevelInput
{
    internal class GamePadManager
    {
        internal const int MAX_GAMEPADS = 4;

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

        internal void EnsurePluggedIn(int gamePadIndex)
        {
            if (pluggedInGamePads[gamePadIndex])
                return;

            SimGamePad.Instance.PlugIn(gamePadIndex);
            pluggedInGamePads[gamePadIndex] = true;
        }

        internal int[] GetAllGamePadIndices()
        {
            return Enumerable.Range(0, MAX_GAMEPADS).ToArray();
        }

        internal void EnsureUnplugged(int gamePadIndex)
        {
            if (!pluggedInGamePads[gamePadIndex])
                return;

            SimGamePad.Instance.Unplug(gamePadIndex);
            pluggedInGamePads[gamePadIndex] = false;
        }

        internal void EnsureAllUnplugged()
        {
            for (int i = 0; i < MAX_GAMEPADS; i++)
                EnsureUnplugged(i);
        }
    }
}
