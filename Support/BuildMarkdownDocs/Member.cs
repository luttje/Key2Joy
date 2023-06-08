using BuildMarkdownDocs.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace BuildMarkdownDocs
{
    internal abstract class Member : IComparable<Member>
    {
        public MarkdownMeta Parent { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }

        internal abstract string GetLinkMarkdown();
        internal virtual void FillTemplateReplacements(ref Dictionary<string, string> replacements)
        {
            replacements.Add("Name", Name);
            replacements.Add("Summary", Summary);
        }

        internal string FillTemplate(string fileTemplate)
        {
            var replacements = new Dictionary<string, string>();
            
            FillTemplateReplacements(ref replacements);

            var template = fileTemplate;

            foreach (var kvp in replacements)
            {
                template = template.Replace($"{{{{{kvp.Key}}}}}", kvp.Value);

                var regex = $@"\{{\{{#if\({kvp.Key}\)\}}\}}(.*)\{{\{{#endif\({kvp.Key}\)\}}\}}";
                var match = Regex.Match(template, regex, RegexOptions.Singleline);

                if (match.Success)
                {
                    var replacement = "";

                    // Removes the if-statement start and end
                    if (kvp.Value.Length > 0)
                        replacement = match.Groups[1].Value;

                    template = Regex.Replace(template, regex, replacement, RegexOptions.Singleline);
                }
            }

            return template;
        }
        
        public virtual int CompareTo(Member other)
        {
            return Name.CompareTo(other.Name);
        }
    }
}