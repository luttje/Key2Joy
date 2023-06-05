using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Key2Joy.Contracts.Plugins
{
    public abstract class AbstractPluginForm : Form
    {
        public Dictionary<string, object> MappingConfigValues { get; } = new();
    }
}
