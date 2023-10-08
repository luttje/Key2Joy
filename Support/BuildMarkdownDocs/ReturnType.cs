using System.Xml.Linq;

namespace BuildMarkdownDocs;

internal class ReturnType
{
    public string Description { get; set; }
    // TODO:
    // public Type Type { get; set; }

    public static ReturnType FromXml(XElement element)
    {
        ReturnType returnType = new()
        {
            Description = element.Value
        };

        return returnType;
    }
}
