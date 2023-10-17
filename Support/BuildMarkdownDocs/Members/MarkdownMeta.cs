using System;
using System.Xml.Linq;

namespace BuildMarkdownDocs.Members;

internal struct MarkdownMeta : IComparable<MarkdownMeta>
{
    public string Name { get; set; }
    public string Path { get; set; }
    public int LevelModifier { get; set; }

    public static MarkdownMeta FromXml(XElement element)
    {
        MarkdownMeta parent = new()
        {
            Name = element.Element("parent-name")?.Value,
            Path = element.Element("path")?.Value
        };

        if (parent.Path.Length > 0)
        {
            parent.Path = !parent.Path.EndsWith("/") ? $"{parent.Path}/" : parent.Path;
        }

        return parent;
    }

    public readonly int CompareTo(MarkdownMeta other) => this.Name.CompareTo(other.Name);
}
