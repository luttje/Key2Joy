using System.IO;
using System.Text.RegularExpressions;

namespace BuildMarkdownDocs.Util;

internal class FileHelper
{
    /// <summary>
    /// Replace different newline characters with LF
    /// </summary>
    /// <param name="text"></param>
    internal static string NormalizeNewlinesToLF(string text)
        => Regex.Replace(text, @"\r\n?|\n", "\n");

    /// <summary>
    /// Writes to the given file, normalizing newlines
    /// </summary>
    /// <param name="stringBuilder"></param>
    internal static void WriteToFile(string filePath, string text)
        => File.WriteAllText(filePath, NormalizeNewlinesToLF(text));
}
