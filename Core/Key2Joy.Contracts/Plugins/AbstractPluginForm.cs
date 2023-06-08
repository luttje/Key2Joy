using Key2Joy.Contracts.Mapping;
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
        public virtual int DesiredHeight { get; protected set; }

        public AbstractPluginForm()
            :base()
        {
            FormBorderStyle = FormBorderStyle.None;
        }
    }
}
