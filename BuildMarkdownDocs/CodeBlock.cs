using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BuildMarkdownDocs
{
    internal class CodeBlock
    {
        public string Language { get; set; }
        public string Code { get; set; }
        
        internal static CodeBlock FromXml(XElement element)
        {
            var code = new CodeBlock();
            code.Language = element.Attribute("language").Value;

            var whiteSpaceToTrimPattern = new Regex(@"^[^\S\r\n]+", RegexOptions.Multiline);
            var whiteSpaceToTrim = whiteSpaceToTrimPattern.Match(element.Value);

            var whiteSpaceTrimPattern = new Regex($"^{whiteSpaceToTrim}", RegexOptions.Multiline);
            code.Code = whiteSpaceTrimPattern.Replace(element.Value, "").Trim();

            return code;
        }

        public override string ToString()
        {
            return $"```{Language}\n{Code}\n```";
        }
    }
}
