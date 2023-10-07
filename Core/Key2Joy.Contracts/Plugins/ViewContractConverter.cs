using Key2Joy.Contracts.Mapping;
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
        // TODO: Just here for testing
        public event EventHandler OnEventTest;
        // TODO: END
        
        private object instance;
        
        public INativeHandleContract ConvertToContract(FrameworkElement element)
        {
            var contract = FrameworkElementAdapters.ViewToContractAdapter(element);
            return contract;
        }

        public INativeHandleContract ConvertToContract(ObjectHandle controlHandle)
        {
            instance = controlHandle.Unwrap();

            // TODO: Just here for testing
            if (instance is IActionOptionsControl instanceAsActionControl) 
            {
                instanceAsActionControl.OptionsChanged += (s, e) => OnEventTest?.Invoke(this, EventArgs.Empty);
            }
            // TODO: END

            return ConvertToContract((FrameworkElement)instance);
        }

        public object RemoteInvoke(string methodName, object[] parameters)
        {
            var type = instance.GetType();
            var mi = type.GetMethod(methodName);
            return mi.Invoke(instance, parameters);
        }

        private RemoteInvokee handler;
        public void AddRemoteEventHandler(string eventName, RemoteInvokee handler)
        {
            this.handler = handler;
            OnEventTest += ViewContractConverter_OnEventTest;
        }

        private void ViewContractConverter_OnEventTest(object sender, EventArgs e)
        {
            handler?.Invoke();
        }
    }
}
