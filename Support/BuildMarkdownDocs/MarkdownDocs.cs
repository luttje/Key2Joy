using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using BuildMarkdownDocs.Util;

namespace BuildMarkdownDocs;

internal class MarkdownDocs
{
    /// <summary>
    /// Builds the individual files, returning a string to append to the index.
    /// </summary>
    /// <param name="xmlFile"></param>
    /// <param name="assemblyDirectory"></param>
    /// <param name="outputDirectory"></param>
    /// <param name="indexBuilder"></param>
    /// <param name="templateFile"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    internal static void BuildWithIndex(string xmlFile, string assemblyDirectory, string outputDirectory, StringBuilder indexBuilder, string templateFile = null, string filter = null)
    {
        var xml = XDocument.Load(xmlFile);
        var root = xml.Root;
        var membersContainer = root.Element("members");
        var xmlMembers = membersContainer.Elements("member");
        outputDirectory = !outputDirectory.EndsWith("\\") ? $"{outputDirectory}\\" : outputDirectory;

        SortedDictionary<MarkdownMeta, List<Member>> outputParents = new();

        var assemblyName = root.Element("assembly").Element("name").Value;
        AssemblyHelper.LoadWithRelated(assemblyDirectory, assemblyName, out var isPlugin);

        MarkdownMeta enumParent;
        List<Member> enumerations;
        outputParents.Add(
            enumParent = new MarkdownMeta()
            {
                Name = "Enumerations",
                Path = "Api/Enumerations/",
                LevelModifier = isPlugin ? 1 : 0
            },
            enumerations = new List<Member>()
        );

        foreach (var xmlMember in xmlMembers)
        {
            if (filter != null)
            {
                var child = xmlMember.XPathSelectElement(filter);

                if (child == null)
                {
                    continue;
                }
            }

            var member = (FunctionMember)FunctionMember.FromXml(xmlMember, isPlugin);
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
                    {
                        continue;
                    }

                    var enumName = parameter.Type.Name;
                    var isTypeDescribed = enumerations.Exists(m =>
                    {
                        var enumMember = (EnumMember)m;
                        return enumMember.Name == enumName;
                    });

                    if (!isTypeDescribed)
                    {
                        EnumMember enumMember;
                        Dictionary<string, string> valueSummaries = new();
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
                        {
                            Directory.CreateDirectory(enumDirectory);
                        }

                        FileHelper.WriteToFile(outputEnumFile, enumMember.FillTemplate(enumTemplateContent));
                    }
                }
            }

            Console.WriteLine($"Writing output to {outputMemberFile}");

            templateFile ??= Path.GetFullPath(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "DefaultTemplate.md"));

            Console.WriteLine($"Using template {templateFile}");

            var templateContent = File.ReadAllText(templateFile);

            var memberDirectory = Path.GetDirectoryName(outputMemberFile);

            if (!Directory.Exists(memberDirectory))
            {
                Directory.CreateDirectory(memberDirectory);
            }

            FileHelper.WriteToFile(outputMemberFile, member.FillTemplate(templateContent));
        }

        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        foreach (var outputParent in outputParents)
        {
            var parent = outputParent.Key;
            var members = outputParent.Value;

            if (members.Count == 0)
            {
                continue;
            }

            members.Sort();

            var headingMarkers = new string('#', parent.LevelModifier + 2);

            indexBuilder.AppendLine($"{headingMarkers} {parent.Name}\n");

            foreach (var member in members)
            {
                indexBuilder.AppendLine(member.GetLinkMarkdown());
            }
            indexBuilder.AppendLine("");
        }
    }
}
