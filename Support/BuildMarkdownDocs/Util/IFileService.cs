using System.Collections.Generic;

namespace BuildMarkdownDocs.Util;

public interface IFileService
{
    string ReadFile(string path);

    void WriteToFile(string path, string content);

    IEnumerable<string> GetXmlFiles(string directory);
}
