using System.IO;
using System.Reflection;
using Key2Joy;

namespace BuildMarkdownDocs;

internal class AssemblyHelper
{
    /// <summary>
    /// Loads all referenced assemblies (so we can get proper type information)
    /// </summary>
    /// <param name="assemblyName"></param>
    /// <param name="assemblyDirectory"></param>
    /// <param name="isPlugin"></param>
    internal static void LoadWithRelated(string assemblyDirectory, string assemblyName, out bool isPlugin)
    {
        var assemblyFileName = $"{assemblyName}.dll";
        var assemblyPath = Path.Combine(assemblyDirectory, assemblyFileName);

        if (!File.Exists(assemblyPath))
        {
            assemblyPath = Path.Combine(assemblyDirectory, Key2JoyManager.PluginsDirectory, assemblyName, assemblyFileName);
            isPlugin = true;
        }
        else
        {
            isPlugin = false;
        }

        var assembly = Assembly.LoadFrom(assemblyPath);

        var referencedAssemblies = assembly.GetReferencedAssemblies();

        foreach (var referencedAssemblyName in referencedAssemblies)
        {
            var referencedAssemblyPath = Path.Combine(assemblyDirectory, $"{referencedAssemblyName.Name}.dll");

            if (File.Exists(referencedAssemblyPath))
            {
                Assembly.LoadFrom(referencedAssemblyPath);
            }
        }
    }
}
