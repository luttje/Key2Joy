using FFMpegCore;
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
        
        public static void InitSafely(AppCommandRunner commandRunner, Action value)
        {
            Key2JoyManager.commandRunner = commandRunner;

            GlobalFFOptions.Configure(options => options.BinaryFolder = "./ffmpeg");

            try
            {
                value();
            }
            finally
            {
                SimGamePad.Instance.ShutDown();
            }
        }

        internal static bool RunAppCommand(AppCommand command)
        {
            return commandRunner(command);
        }
    }
}
