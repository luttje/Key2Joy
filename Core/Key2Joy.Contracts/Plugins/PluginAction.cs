using Key2Joy.Contracts.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Contracts.Plugins
{
    public class PluginAction : MarshalByRefObject
    {
        public virtual MappingAspectOptions BuildSaveOptions(MappingAspectOptions options) => options;
        public virtual void LoadOptions(MappingAspectOptions options) { }

        public virtual string GetNameDisplay(string nameFormat)
        {
            return nameFormat;
        }
    }
}
