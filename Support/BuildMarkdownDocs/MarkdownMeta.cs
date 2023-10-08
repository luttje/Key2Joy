using System;
using System.Xml.Linq;

namespace BuildMarkdownDocs
{
    internal struct MarkdownMeta : IComparable<MarkdownMeta>
    {
        public string Name { get; set; }
        public string Path { get; set; }

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

        public int CompareTo(MarkdownMeta other)
        {
            return this.Name.CompareTo(other.Name);
        }
    }
}
