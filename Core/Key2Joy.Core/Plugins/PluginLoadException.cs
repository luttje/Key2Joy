using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Plugins
{
    internal class PluginLoadException : Exception
    {
        public PluginLoadException(string message) : base(message)
        {
        }
    }
}
