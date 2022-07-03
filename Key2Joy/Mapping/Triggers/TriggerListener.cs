using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Key2Joy.Mapping
{
    public abstract class TriggerListener
    {
        internal virtual bool HasWndProcHandle { get; } = false;
        internal IntPtr Handle { get; set; }

        internal bool IsActive { get; private set; }

        internal void StartListening()
        {
            if (IsActive)
                return;

            Start();
        }
        internal void StopListening()
        {
            if (!IsActive)
                return;

            Stop();
        }

        internal abstract void AddMappedOption(MappedOption mappedOption);

        protected virtual void Start()
        {
            IsActive = true;
        }


        protected virtual void Stop()
        {
            IsActive = false;
        }

        internal virtual void WndProc(ref Message m)
        { }
    }
}
