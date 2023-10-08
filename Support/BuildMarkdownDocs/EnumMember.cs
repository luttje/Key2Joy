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
            return $"* [`{this.Name}`]({this.Parent.Path}{this.Name}.md)";
        }

        internal override void FillTemplateReplacements(ref Dictionary<string, string> replacements)
        {
            base.FillTemplateReplacements(ref replacements);

            StringBuilder allEnumerations = new();
            string firstName = null;

            foreach (var name in Enum.GetNames(this.Type))
            {
                var memberInfo = this.Type.GetMember(name);
                var enumValueMemberInfo = memberInfo.FirstOrDefault(
                    m => m.DeclaringType == this.Type);
                var valueAttributes = enumValueMemberInfo.GetCustomAttribute(typeof(ObsoleteAttribute), false);

                if (valueAttributes != null)
                {
                    continue;
                }

                firstName ??= name;

                var summary = this.ValueSummaries != null && this.ValueSummaries.ContainsKey(name)
                    ? $": {this.ValueSummaries[name]}" : "";

                allEnumerations.AppendLine($"* `{name}`{summary}");
            }

            if (firstName != null)
            {
                replacements.Add("Example", $"`{this.Name}.{firstName}`");
            }

            replacements.Add("Values", allEnumerations.ToString());
        }
    }
}