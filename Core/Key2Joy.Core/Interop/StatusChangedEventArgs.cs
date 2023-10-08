using System;
using Key2Joy.Mapping;

namespace Key2Joy.Interop
{
    public class StatusChangedEventArgs : EventArgs
    {
        public bool IsEnabled { get; set; }
        public MappingProfile Profile { get; set; } = null;
    }
}
