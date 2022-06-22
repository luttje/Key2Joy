using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyToJoy.Mapping
{
    internal abstract class TriggerListener
    {
        internal virtual bool HasWndProcHandle { get; } = false;
        internal IntPtr Handle { get; set; }
        
        internal abstract void Start();
        internal abstract void Stop();
        internal abstract void AddMappedOption(MappedOption mappedOption);

        internal virtual void WndProc(ref Message m)
        { }
    }
}
