using BuildMarkdownDocs.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            string firstName = null;

            foreach (var name in Enum.GetNames(Type))
            {
                var memberInfo = Type.GetMember(name);
                var enumValueMemberInfo = memberInfo.FirstOrDefault(
                    m => m.DeclaringType == Type);
                var valueAttributes = enumValueMemberInfo.GetCustomAttribute(typeof(ObsoleteAttribute), false);

                if (valueAttributes != null)
                    continue;
                
                if (firstName == null)
                    firstName = name;
                
                allEnumerations.AppendLine($"* `{name}`");
            }

            if(firstName != null)
            replacements.Add("Example", $"`{Name}.{firstName}`");
            replacements.Add("Values", allEnumerations.ToString());
        }
    }
}