using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Contracts.Plugins
{
    public class RemoteInvokee : MarshalByRefObject
    {
        private Action eventHandler;

        public RemoteInvokee(Action eventHandler)
        {
            this.eventHandler = eventHandler;
        }

        public void Invoke()
        {
            eventHandler();
        }
    }
}
