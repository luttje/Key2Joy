using FFMpegCore;
using Key2Joy.Interop;
using Key2Joy.Mapping;
using SimWinInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy
{
    public delegate bool AppCommandRunner(AppCommand command);
        
    public class Key2JoyManager
    {
        private static AppCommandRunner commandRunner;

        /// <summary>
        /// Ensures Key2Joy is running and ready to accept commands as long as the main loop does not end.
        /// </summary>
        public static void InitSafely(AppCommandRunner commandRunner, Action mainLoop)
        {
            Key2JoyManager.commandRunner = commandRunner;

            GlobalFFOptions.Configure(options => options.BinaryFolder = "./ffmpeg");

            try
            {
                InteropServer.Instance.RestartListening();
                mainLoop();
            }
            finally
            {
                InteropServer.Instance.StopListening();
                SimGamePad.Instance.ShutDown();
            }
        }

        internal static bool RunAppCommand(AppCommand command)
        {
            return commandRunner(command);
        }
    }
}
