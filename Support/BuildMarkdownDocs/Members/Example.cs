using System.Collections.Generic;
using System.Xml.Linq;
using BuildMarkdownDocs.Util;

namespace BuildMarkdownDocs.Members;

internal class Example
{
    public List<string> TextContent { get; set; }

    public Example() => this.TextContent = new List<string>();

    internal static Example FromXml(XElement element)
    {
        Example example = new();
        var children = element.Nodes();

        foreach (var child in children)
        {
            var value = "";

            if (child is XText textChild)
            {
                value = textChild.Value.Trim().TrimEachLine().TrimEndEachLine();
            }
            else if (child is XElement elementChild)
            {
                if (elementChild.Name.LocalName == "code")
                {
                    value = CodeBlock.FromXml(elementChild).ToString();
                }
                else
                {
                    value = elementChild.Value.Trim();
                }
            }

            example.TextContent.AddRange(value.TrimEndEachLine().Split('\n'));
        }

        return example;
    }

    public override string ToString() => "> " + string.Join("\n> ", this.TextContent) + "\n---";
}
