using System;
using System.AddIn.Contract;
using System.AddIn.Pipeline;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Resources.ResXFileRef;

namespace Key2Joy.Contracts.Plugins
{
    public class ViewContractConverter : MarshalByRefObject
    {
        private object instance;
        
        public INativeHandleContract ConvertToContract(FrameworkElement element)
        {
            var contract = FrameworkElementAdapters.ViewToContractAdapter(element);
            return contract;
        }

        public INativeHandleContract ConvertToContract(ObjectHandle controlHandle)
        {
            instance = controlHandle.Unwrap();
            return ConvertToContract((FrameworkElement)instance);
        }

        public void RemoteInvoke(string methodName, object[] parameters)
        {
            var type = instance.GetType();
            var mi = type.GetMethod(methodName);
            mi.Invoke(instance, parameters);
        }
    }
}
