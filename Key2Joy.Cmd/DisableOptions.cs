using CommandLine.Text;
using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Key2Joy.Interop;

namespace Key2Joy.Cmd
{
    [Verb("disable", HelpText = "Disable whichever mapping profile is active.")]
    internal class DisableOptions : Options
    {
        public override void Handle()
        {
            Console.WriteLine("Commanding Key2Joy to disable the active profile.");
            
            try
            {
                InteropClient.Instance.SendCommand(new DisableCommand());
            }
            catch (TimeoutException)
            {
                SafelyRetry(() =>
                {
                    Console.WriteLine("Key2Joy is not running, starting it now.");
                    Program.StartKey2Joy();
                    Handle();
                });
            }
        }
    }
}
