using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Plugins;
using Key2Joy.PluginHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace Key2Joy.Plugins
{
    public class ElementHostProxy : ElementHost, IActionOptionsControl
    {
        public event EventHandler OptionsChanged;
        
        private NativeHandleContractInsulator contract;

        public ElementHostProxy(FrameworkElement child, NativeHandleContractInsulator contract)
            : base()
        {
            this.Child = child;
            this.contract = contract;
        }

        public void OnOptionsChanged()
        {
            this.Invoke((MethodInvoker)delegate
            {
                OptionsChanged?.Invoke(null, EventArgs.Empty);
            });
        }

        public bool CanMappingSave(object action)
        {
            var realAction = ((PluginActionProxy)action).GetRealObject();
            return (bool) contract.RemoteInvokeUI(nameof(CanMappingSave), new object[] { realAction });
        }

        public void Select(object action)
        {
            var realAction = ((PluginActionProxy)action).GetRealObject();
            contract.RemoteInvokeUI(nameof(Select), new object[] { realAction });
        }

        public void Setup(object action)
        {
            var realAction = ((PluginActionProxy)action).GetRealObject();
            contract.RemoteInvokeUI(nameof(Setup), new object[] { realAction });

            // TODO: This doesnt work yet, but perhaps this will help us:
            // https://www.codeproject.com/Articles/13847/Two-way-Remoting-with-Callbacks-and-Events-Explain
            contract.AddRemoteEventHandler("OptionsChanged", new RemoteInvokee(OnOptionsChanged));
        }
    }
}
