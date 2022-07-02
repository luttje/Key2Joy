using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BuildMarkdownDocs
{
    internal class ReturnType
    {
        public string Description { get; set; }

        public static ReturnType FromXml(XElement element)
        {
            var returnType = new ReturnType();
            returnType.Description = element.Value;

            return returnType;
        }
    }
}
