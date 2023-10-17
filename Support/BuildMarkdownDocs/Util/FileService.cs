using System.Collections.Generic;
using System.IO;

namespace BuildMarkdownDocs.Util;

public class FileService : IFileService
{
    public string ReadFile(string path)
        => File.ReadAllText(path);

    public void WriteToFile(string path, string content)
    {
        var directory = Path.GetDirectoryName(path);

        if (!string.IsNullOrWhiteSpace(directory)
            && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(path, content.NormalizeNewlinesToLF());
    }

    public IEnumerable<string> GetXmlFiles(string directory)
        => Directory.GetFiles(directory, "*.xml");
}
