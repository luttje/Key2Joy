using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BuildMarkdownDocs
{
    internal class EnumMember : Member
    {
        public Type Type { get; set; }
        public Dictionary<string, string> ValueSummaries;

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

                var summary = ValueSummaries != null && ValueSummaries.ContainsKey(name)
                    ? $": {ValueSummaries[name]}" : "";

                allEnumerations.AppendLine($"* `{name}`{summary}");
            }

            if (firstName != null)
                replacements.Add("Example", $"`{Name}.{firstName}`");
            replacements.Add("Values", allEnumerations.ToString());
        }
    }
}