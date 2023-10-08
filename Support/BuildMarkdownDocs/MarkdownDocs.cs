using BuildMarkdownDocs.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;

namespace BuildMarkdownDocs
{
    internal class MarkdownDocs
    {
        internal static void Build(string xmlFile, string outputDirectory, string templateFile = null, string filter = null)
        {
            var xml = XDocument.Load(xmlFile);
            var root = xml.Root;
            var membersContainer = root.Element("members");
            var xmlMembers = membersContainer.Elements("member");
            outputDirectory = !outputDirectory.EndsWith("\\") ? $"{outputDirectory}\\" : outputDirectory;

            var outputFile = Path.GetFullPath(outputDirectory + "Index.md");
            var outputParents = new SortedDictionary<MarkdownMeta, List<Member>>();

            MarkdownMeta enumParent;
            List<Member> enumerations;
            outputParents.Add(
                enumParent = new MarkdownMeta()
                {
                    Name = "All Enumerations",
                    Path = "Api/Enumerations/"
                },
                enumerations = new List<Member>()
            );

            var directory = Path.GetDirectoryName(outputFile);

            foreach (var xmlMember in xmlMembers)
            {
                if (filter != null)
                {
                    var child = xmlMember.XPathSelectElement(filter);

                    if (child == null)
                        continue;
                }

                var member = (FunctionMember)FunctionMember.FromXml(xmlMember);
                var outputMemberFile = Path.GetFullPath(outputDirectory + member.Parent.Path + member.Name.FirstCharToUpper() + ".md");

                if (!outputParents.TryGetValue(member.Parent, out var parent))
                {
                    outputParents.Add(member.Parent, parent = new List<Member>());
                }

                parent.Add(member);

                if (member.Parameters != null)
                {
                    foreach (var parameter in member.Parameters)
                    {
                        if (!parameter.Type.IsEnum)
                            continue;

                        var enumName = parameter.Type.Name;
                        var isTypeDescribed = enumerations.Exists(m =>
                        {
                            var enumMember = (EnumMember)m;
                            return enumMember.Name == enumName;
                        });

                        if (!isTypeDescribed)
                        {
                            EnumMember enumMember;
                            var valueSummaries = new Dictionary<string, string>();
                            var fullXmlName = $"F:{parameter.Type.FullName}";

                            foreach (var m in xmlMembers)
                            {
                                var name = m.Attribute("name").Value;

                                if (name.StartsWith(fullXmlName))
                                {
                                    var summary = m.Element("summary").Value.TrimEachLine().Trim('\n');
                                    valueSummaries.Add(name.Substring(fullXmlName.Length + 1), summary);
                                }
                            }

                            enumerations.Add(enumMember = new EnumMember()
                            {
                                Parent = enumParent,
                                Type = parameter.Type,
                                Name = enumName,
                                Summary = "",
                                ValueSummaries = valueSummaries,
                            });

                            var outputEnumFile = Path.GetFullPath(outputDirectory + enumMember.Parent.Path + enumMember.Name.FirstCharToUpper() + ".md");
                            var enumTemplateFile = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "DefaultEnumTemplate.md"));
                            var enumTemplateContent = File.ReadAllText(enumTemplateFile);
                            var enumDirectory = Path.GetDirectoryName(outputEnumFile);

                            if (!Directory.Exists(enumDirectory))
                                Directory.CreateDirectory(enumDirectory);

                            File.WriteAllText(outputEnumFile, enumMember.FillTemplate(enumTemplateContent));
                        }
                    }
                }

                Console.WriteLine($"Writing output to {outputMemberFile}");

                if (templateFile == null)
                    templateFile = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "DefaultTemplate.md"));

                Console.WriteLine($"Using template {templateFile}");

                var templateContent = File.ReadAllText(templateFile);

                var memberDirectory = Path.GetDirectoryName(outputMemberFile);

                if (!Directory.Exists(memberDirectory))
                    Directory.CreateDirectory(memberDirectory);

                File.WriteAllText(outputMemberFile, member.FillTemplate(templateContent));
            }

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var output = new StringBuilder();
            output.AppendLine($"# Scripting API Reference\n");

            // Sort members
            foreach (var outputParent in outputParents)
            {
                var parent = outputParent.Key;
                var members = outputParent.Value;
                members.Sort();

                output.AppendLine($"## {parent.Name}\n");

                foreach (var member in members)
                {
                    output.AppendLine(member.GetLinkMarkdown());
                }
                output.AppendLine("");
            }

            File.WriteAllText(outputFile, output.ToString());
        }
    }
}
