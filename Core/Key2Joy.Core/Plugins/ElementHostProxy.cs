using Key2Joy.Contracts.Mapping;
using Key2Joy.PluginHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        public bool CanMappingSave(AbstractAction action)
        {
            // TODO: return ((IActionOptionsControl)Child).CanMappingSave(action);
            contract.RemoteInvokeUI(nameof(CanMappingSave), new object[] { action }); 
            // How do we get the return value if its executed by a dispatcher across another process and appdomain
            
            return false;
        }

        public void Select(AbstractAction action)
        {
            contract.RemoteInvokeUI(nameof(Select), new object[] { action });
        }

        public void Setup(AbstractAction action)
        {
            contract.RemoteInvokeUI(nameof(Setup), new object[] { action });
        }
    }
}
