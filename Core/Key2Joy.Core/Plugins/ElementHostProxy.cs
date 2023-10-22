using System;
using System.Windows;
using System.Windows.Forms.Integration;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.PluginHost;

namespace Key2Joy.Plugins;

public class ElementHostProxy : ElementHost, IActionOptionsControl
{
    public event EventHandler OptionsChanged;

    private readonly NativeHandleContractInsulator contract;

    public ElementHostProxy(FrameworkElement child, NativeHandleContractInsulator contract)
        : base()
    {
        this.Child = child;
        this.contract = contract;
    }

    public void InvokeOptionsChanged() => OptionsChanged?.Invoke(this, EventArgs.Empty);

    public bool CanMappingSave(AbstractAction action)
    {
        var realAction = ((PluginActionProxy)action).GetRealObject();
        return (bool)this.contract.RemoteInvokeUI(nameof(CanMappingSave), new object[] { realAction });
    }

    public void Select(AbstractAction action)
    {
        var realAction = ((PluginActionProxy)action).GetRealObject();
        this.contract.RemoteInvokeUI(nameof(Select), new object[] { realAction });
    }

    public void Setup(AbstractAction action)
    {
        var realAction = ((PluginActionProxy)action).GetRealObject();
        this.contract.RemoteInvokeUI(nameof(Setup), new object[] { realAction });
    }

    public int GetDesiredHeight() => (int)this.contract.RemoteInvokeUI(nameof(GetDesiredHeight), new object[] { });
}
