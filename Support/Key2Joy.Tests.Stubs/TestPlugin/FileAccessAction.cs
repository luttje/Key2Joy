using System.IO;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Plugins;

namespace Key2Joy.Tests.Stubs.TestPlugin;

[Action(
    Description = "FileAccessAction",
    Visibility = MappingMenuVisibility.Never,
    NameFormat = "FileAccessAction"
)]
public class FileAccessAction : PluginAction
{
    public void WriteToDataDirectory()
    {
        var fileWriter = File.AppendText(Path.Combine(this.Plugin.PluginDataDirectory, "test.txt"));
        fileWriter.AutoFlush = true;
        fileWriter.WriteLine($"Hello World!");
        fileWriter.Close();
    }

    public void WriteToDataDirectoryWithUnsafePath(string relativePath)
    {
        var fileWriter = File.AppendText(Path.Combine(this.Plugin.PluginDataDirectory, relativePath, "test.txt"));
        fileWriter.AutoFlush = true;
        fileWriter.WriteLine($"Hello World!");
        fileWriter.Close();
    }

    public void WriteToAbsolutePath(string path)
    {
        var fileWriter = File.AppendText(path);
        fileWriter.AutoFlush = true;
        fileWriter.WriteLine($"Hello World!");
        fileWriter.Close();
    }

    public string ReadAbsolutePath(string path) => File.ReadAllText(path);
}
