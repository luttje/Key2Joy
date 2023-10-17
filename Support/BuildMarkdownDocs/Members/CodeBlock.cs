using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace BuildMarkdownDocs.Members;

internal class CodeBlock
{
    public string Language { get; set; }
    public string Code { get; set; }

    internal static CodeBlock FromXml(XElement element)
    {
        CodeBlock code = new()
        {
            Language = element.Attribute("language").Value
        };

        Regex whiteSpaceToTrimPattern = new(@"^[^\S\r\n]+", RegexOptions.Multiline);
        var whiteSpaceToTrim = whiteSpaceToTrimPattern.Match(element.Value);

        Regex whiteSpaceTrimPattern = new($"^{whiteSpaceToTrim}", RegexOptions.Multiline);
        code.Code = whiteSpaceTrimPattern.Replace(element.Value, "").Trim();

        return code;
    }

    public override string ToString() => $"\n#### _{this.Language}_:\n```{this.Language}\n{this.Code}\n```";
}
