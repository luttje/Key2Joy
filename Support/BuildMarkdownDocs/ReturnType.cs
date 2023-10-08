using System.Xml.Linq;

namespace BuildMarkdownDocs
{
    internal class ReturnType
    {
        public string Description { get; set; }
        // TODO:
        // public Type Type { get; set; }

        public static ReturnType FromXml(XElement element)
        {
            var returnType = new ReturnType();
            returnType.Description = element.Value;

            return returnType;
        }
    }
}
