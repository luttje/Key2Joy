using Key2Joy.LowLevelInput;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    internal interface IPressState
    {
        PressState PressState { get; set; }
    }
}
