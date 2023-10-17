namespace BuildMarkdownDocs.Util;

public class CommandLineArgs
{
    public string AssemblyDirectory { get; set; }
    public string XmlFilesDirectory { get; set; }
    public string OutputDirectory { get; set; }
    public string Template { get; set; }
    public string Filter { get; set; }
}
