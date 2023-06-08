using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Contracts.Mapping
{
    public class TriggerExecutingEventArgs : EventArgs
    {
        public bool Handled { get; set; }

        public TriggerExecutingEventArgs()
        {
        }
    }
}
