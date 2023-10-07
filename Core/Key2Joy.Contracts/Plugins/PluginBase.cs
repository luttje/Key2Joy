using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Contracts.Plugins
{
    public abstract class PluginBase : MarshalByRefObject
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract string Author { get; }
        public abstract string Website { get; }

        public string PluginDataDirectory { get; set; }

        public virtual void Initialize()
        { }
    }
}
