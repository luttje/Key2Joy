using BuildMarkdownDocs.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace BuildMarkdownDocs
{
    internal class EnumMember : Member
    {
        public Type Type { get; set; }
        
        internal override string GetLinkMarkdown()
        {
            return $"* [`{Name}`]({Parent.Path}{Name}.md)";
        }

        internal override void FillTemplateReplacements(ref Dictionary<string, string> replacements)
        {
            base.FillTemplateReplacements(ref replacements);

            var allEnumerations = new StringBuilder();

            foreach (var name in Enum.GetNames(Type))
            {
                allEnumerations.AppendLine($"* `{name}`");
            }

            replacements.Add("Values", allEnumerations.ToString());
        }
    }
}