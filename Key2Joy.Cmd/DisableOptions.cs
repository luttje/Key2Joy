using CommandLine.Text;
using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Cmd
{
    [Verb("disable", HelpText = "Disable whichever preset profile is active.")]
    internal class DisableOptions : Options
    {
        public override void Handle()
        {
            throw new NotImplementedException();
        }
    }
}
