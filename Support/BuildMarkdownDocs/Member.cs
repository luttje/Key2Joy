using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BuildMarkdownDocs;

internal abstract class Member : IComparable<Member>
{
    public MarkdownMeta Parent { get; set; }
    public string Name { get; set; }
    public string Summary { get; set; }

    internal abstract string GetLinkMarkdown();
    internal virtual void FillTemplateReplacements(ref Dictionary<string, string> replacements)
    {
        replacements.Add("Name", this.Name);
        replacements.Add("Summary", this.Summary);
    }

    internal string FillTemplate(string fileTemplate)
    {
        Dictionary<string, string> replacements = new();

        this.FillTemplateReplacements(ref replacements);

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
                {
                    replacement = match.Groups[1].Value;
                }

                template = Regex.Replace(template, regex, replacement, RegexOptions.Singleline);
            }
        }

        return template;
    }

    public virtual int CompareTo(Member other) => this.Name.CompareTo(other.Name);
}