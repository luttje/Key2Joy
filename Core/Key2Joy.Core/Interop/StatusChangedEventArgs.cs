using Key2Joy.Mapping;
using System;

namespace Key2Joy.Interop
{
    public class StatusChangedEventArgs : EventArgs
    {
        public bool IsEnabled { get; set; }
        public MappingProfile Profile { get; set; } = null;
    }
}
