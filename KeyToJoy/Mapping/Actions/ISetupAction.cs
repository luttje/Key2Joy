using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyToJoy.Mapping
{
    internal interface ISetupAction
    {
        void Setup(BaseAction action);
    }
}
