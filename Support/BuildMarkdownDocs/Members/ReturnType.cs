using System;
using System.Reflection;
using System.Xml.Linq;

namespace BuildMarkdownDocs.Members;

internal class ReturnType
{
    public string Description { get; set; }
    public Type Type { get; set; }

    public static ReturnType FromXml(XElement element, MethodInfo methodInfo)
    {
        ReturnType returnType = new()
        {
            Description = element.Value,
            Type = methodInfo.ReturnType
        };

        return returnType;
    }

    internal object GetTypeName()
        => this.Type.Name;
}
