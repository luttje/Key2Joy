using System;
using System.Text;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Plugins;
using Microsoft.Win32;

namespace Key2Joy.Tests.Stubs.TestPlugin;

[Action(
    Description = "RegistryAccessAction",
    Visibility = MappingMenuVisibility.Never,
    NameFormat = "RegistryAccessAction"
)]
public class RegistryAccessAction : PluginAction
{
    public string ReadRegistrySubKeyNames(string keyPath)
    {
        var stringBuilder = new StringBuilder();

        using (var key = Registry.LocalMachine.OpenSubKey(keyPath))
        {
            var subKeyNames = key.GetSubKeyNames();

            foreach (var subKeyName in subKeyNames)
            {
                stringBuilder.AppendLine(subKeyName);
            }
        }

        return stringBuilder.ToString();
    }
}
