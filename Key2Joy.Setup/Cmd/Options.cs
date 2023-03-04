
using CommandLine.Text;
using CommandLine;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Key2Joy.Setup.Cmd
{
    internal abstract class Options
    {
        abstract public void Handle();
    }
}
