using System.Collections.Generic;
using System.IO;

namespace BuildMarkdownDocs.Util;

internal class FileService
{
    public static string ReadFile(string path)
        => File.ReadAllText(path);

    public static void WriteToFile(string path, string content)
    {
        var directory = Path.GetDirectoryName(path);

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(path, content.NormalizeNewlinesToLF());
    }

    public static IEnumerable<string> GetXmlFiles(string directory)
        => Directory.GetFiles(directory, "*.xml");
}
