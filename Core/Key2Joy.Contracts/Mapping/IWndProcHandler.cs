using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Contracts.Mapping
{
    public struct Message
    {
        public IntPtr HWnd { get; private set; }

        public int Msg { get; private set; }

        public IntPtr WParam { get; private set; }

        public IntPtr LParam { get; private set; }

        public Message(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
        {
            HWnd = hWnd;
            Msg = msg;
            WParam = wParam;
            LParam = lParam;
        }
    }
    
    public interface IWndProcHandler
    {
        public IntPtr Handle { get; set; }

        public void WndProc(Message message);
    }
}
