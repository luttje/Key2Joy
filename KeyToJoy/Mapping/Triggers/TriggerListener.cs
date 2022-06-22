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

        internal bool IsStarted { get; private set; }

        internal void StartIfNotStarted()
        {
            if (IsStarted)
                return;

            Start();
        }
        internal void StopIfNotStopped()
        {
            if (!IsStarted)
                return;

            Stop();
        }


        protected virtual void Start()
        {
            IsStarted = true;
        }


        protected virtual void Stop()
        {
            IsStarted = false;
        }

        internal abstract void AddMappedOption(MappedOption mappedOption);

        internal virtual void WndProc(ref Message m)
        { }
    }
}
