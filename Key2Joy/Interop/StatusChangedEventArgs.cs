using Key2Joy.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Interop
{
    public class StatusChangedEventArgs : EventArgs
    {
        public bool IsEnabled { get; set; }
        public MappingPreset Preset { get; set; } = null;
    }
}
