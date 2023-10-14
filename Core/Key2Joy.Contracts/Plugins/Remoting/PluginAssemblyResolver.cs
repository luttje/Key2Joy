using System;
using System.IO;
using System.Reflection;

namespace Key2Joy.Contracts.Plugins.Remoting;

public class PluginAssemblyResolver : MarshalByRefObject
{
    private string pluginDirectory;

    public void SetPluginDirectory(string pluginDirectory) => this.pluginDirectory = pluginDirectory;

    public Assembly ResolveAssembly(object sender, ResolveEventArgs args)
    {
        var assemblyName = new AssemblyName(args.Name);
        var assemblyPath = Path.Combine(pluginDirectory, assemblyName.Name + ".dll");

        if (File.Exists(assemblyPath))
        {
            return Assembly.LoadFrom(assemblyPath);
        }

        throw new NotImplementedException(assemblyPath);

        return null;
    }
}
