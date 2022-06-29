using BuildMarkdownDocs.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace BuildMarkdownDocs
{
    internal class Member : IComparable<Member>
    {
        public MarkdownMeta Parent { get; set; }
        public Example[] MarkdownExamples { get; set; }
        
        public string Name { get; set; }
        public string Summary { get; set; }
        public Parameter[] Parameters { get; set; }

        public string GetParametersSignature()
        {
            if (Parameters == null)
                return string.Empty;
            
            return string.Join(", ", Parameters?
                    .Select(p => $"`{p.Type?.Name}`"));
        }

        internal static Member FromXml(XElement element)
        {
            // Get the parameter types from member attribute: <member name="M:KeyToJoy.Mapping.KeyboardAction.ExecuteForScript(System.Windows.Forms.Keys,KeyToJoy.Input.PressState)">
            var memberName = element.Attribute("name").Value;
            var parametersStart = memberName.IndexOf('(');
            Type[] parameterTypes;

            if (parametersStart > -1)
            {
                var parametersEnd = memberName.LastIndexOf(')')-1;
                var parameters = memberName.Substring(parametersStart+1, memberName.Length - parametersStart - (memberName.Length - parametersEnd));
                parameterTypes = parameters.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(typeName => TypeUtil.GetType(typeName))
                    .ToArray();
            }
            else
            {
                parameterTypes = new Type[0];
            }

            var member = new Member();
            member.Name = element.Element("name")?.Value ?? element.Attribute("name").Value;
            
            var summaryNodes = element.Element("summary").Nodes();
            var summary = new StringBuilder();

            foreach (var node in summaryNodes)
            {
                if (node is XElement nodeElement)
                {
                    if(nodeElement.Name == "see")
                    {
                        var href = nodeElement.Attribute("href")?.Value;
                        var cref = nodeElement.Attribute("cref")?.Value;

                        if(href != null)
                            summary.Append($" [{href}]({href}) ");
                        else
                            summary.Append($" `{cref}` ");
                    }
                    else
                        summary.Append(nodeElement.Value.TrimEachLine());
                }
                else
                    summary.Append(node.ToString().TrimEachLine());
            }

            member.Summary = summary.ToString();

            var i = 0;
            if (parameterTypes.Length > 0)
                member.Parameters = element.Elements("param")?
                    .Select(e => Parameter.FromXml(e, parameterTypes[i++]))
                    .ToArray();

            var markdownMeta = element.Element("markdown-doc");

            member.Parent = MarkdownMeta.FromXml(markdownMeta);
            member.MarkdownExamples = element.Elements("markdown-example")
                .Select(e => Example.FromXml(e))
                .ToArray();
            
            return member;
        }

        internal string FillTemplate(string fileTemplate)
        {
            var parametersSignature = GetParametersSignature();
            var parameters = "";
            
            if(Parameters != null)
            {
                parameters = string.Join("\n", Parameters?
                    .Select(p => $"* **{p.Name} (`{p.Type.Name}`)** \n\n" +
                        $"\t{p.Description}\n"));
            }
            
            var examples = string.Join<Example>("\n\n", MarkdownExamples);

            var replacements = new Dictionary<string, string>()
            {
                { "Name", Name },
                { "ParametersSignature", parametersSignature },
                { "Summary", Summary },
                { "Parameters", parameters },
                { "Examples", examples },
            };

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

        public int CompareTo(Member other)
        {
            return Name.CompareTo(other.Name);
        }
    }
}