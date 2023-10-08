using BuildMarkdownDocs.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace BuildMarkdownDocs
{
    internal class FunctionMember : Member
    {
        public Example[] MarkdownExamples { get; set; }
        public Parameter[] Parameters { get; set; }
        public ReturnType ReturnType { get; set; }

        public string GetParametersSignature()
        {
            if (Parameters == null)
            {
                return string.Empty;
            }

            return string.Join(", ", Parameters?
                    .Select(p => $"`{p.GetTypeName()}`"));
        }

        internal static Member FromXml(XElement element)
        {
            // Get the parameter types from member attribute: <member name="M:Key2Joy.Mapping.KeyboardAction.ExecuteForScript(System.Windows.Forms.Keys,Key2Joy.Input.PressState)">
            var memberName = element.Attribute("name").Value;
            var parametersStart = memberName.IndexOf('(');
            Type[] parameterTypes;

            if (parametersStart > -1)
            {
                var parametersEnd = memberName.LastIndexOf(')') - 1;
                var parameters = memberName.Substring(parametersStart + 1, memberName.Length - parametersStart - (memberName.Length - parametersEnd));
                parameterTypes = parameters.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(typeName => TypeUtil.GetType(typeName))
                    .ToArray();
            }
            else
            {
                parameterTypes = new Type[0];
            }

            FunctionMember member = new()
            {
                Name = element.Element("name")?.Value ?? element.Attribute("name").Value
            };

            var summaryNodes = element.Element("summary").Nodes();
            StringBuilder summary = new();

            foreach (var node in summaryNodes)
            {
                if (node is XElement nodeElement)
                {
                    if (nodeElement.Name == "see")
                    {
                        var href = nodeElement.Attribute("href")?.Value;
                        var cref = nodeElement.Attribute("cref")?.Value;

                        if (href != null)
                        {
                            summary.Append($" [{href}]({href}) ");
                        }
                        else
                        {
                            summary.Append($" `{cref}` ");
                        }
                    }
                    else
                    {
                        summary.Append(nodeElement.Value.TrimEachLine());
                    }
                }
                else
                {
                    summary.Append(node.ToString().TrimEachLine());
                }
            }

            member.Summary = summary.ToString();

            var returnTypeEl = element.Element("returns");
            member.ReturnType = returnTypeEl != null ? ReturnType.FromXml(returnTypeEl) : null;

            var i = 0;
            if (parameterTypes.Length > 0)
            {
                member.Parameters = element.Elements("param")?
                    .Select(e => Parameter.FromXml(e, parameterTypes[i++]))
                    .ToArray();
            }

            var markdownMeta = element.Element("markdown-doc");

            member.Parent = MarkdownMeta.FromXml(markdownMeta);
            member.MarkdownExamples = element.Elements("markdown-example")
                .Select(e => Example.FromXml(e))
                .ToArray();

            return member;
        }

        internal override string GetLinkMarkdown()
        {
            return $"* [`{Name}` ({GetParametersSignature()})]({Parent.Path}{Name}.md)";
        }

        internal override void FillTemplateReplacements(ref Dictionary<string, string> replacements)
        {
            base.FillTemplateReplacements(ref replacements);

            var parametersSignature = GetParametersSignature();
            var parameters = "";

            if (Parameters != null)
            {
                parameters = string.Join("\n", Parameters?
                    .Select(p =>
                        $"* **{p.Name} (" + (p.IsOptional ? "Optional " : "") + $"`{p.GetTypeName(false)}`)** \n\n" +
                        $"\t{p.Description}\n"));
            }

            var examples = string.Join<Example>("\n\n", MarkdownExamples);

            replacements.Add("ParametersSignature", parametersSignature);
            replacements.Add("Parameters", parameters);
            replacements.Add("ReturnType", $"{ReturnType?.Description ?? ""}");
            replacements.Add("Examples", examples);

        }
    }
}