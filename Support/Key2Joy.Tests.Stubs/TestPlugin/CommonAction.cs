using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Plugins;

namespace Key2Joy.Tests.Stubs.TestPlugin;

[Action(
    Description = "CommonAction",
    Visibility = MappingMenuVisibility.Never,
    NameFormat = "CommonAction"
)]
public class CommonAction : PluginAction
{
    public string LastCalledName { get; private set; }

    public void CallWithoutParameters() => this.LastCalledName = nameof(CallWithoutParameters);

    public void CallWithParameter(string target) => this.LastCalledName = nameof(CallWithParameter);

    public object CallWithParameterAndReturn_DoubleInput(string target)
    {
        this.LastCalledName = nameof(CallWithParameterAndReturn_DoubleInput);

        return target + target;
    }
}
