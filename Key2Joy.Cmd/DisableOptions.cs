﻿using CommandLine;
using Key2Joy.Interop;
using System;

namespace Key2Joy.Cmd
{
    [Verb("disable", HelpText = "Disable whichever mapping profile is active.")]
    internal class DisableOptions : Options
    {
        public override void Handle()
        {
            try
            {
                InteropClient.Instance.SendCommand(new DisableCommand());

                Console.WriteLine("Commanded Key2Joy to disable the active profile.");
            }
            catch (TimeoutException)
            {
                SafelyRetry(() =>
                {
                    Console.WriteLine("Key2Joy is not running, starting it now...");
                    Key2JoyManager.StartKey2Joy();
                    Handle();
                });
            }
        }
    }
}
