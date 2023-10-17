using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using BuildMarkdownDocs.Members;
using BuildMarkdownDocs.Util;

namespace BuildMarkdownDocs;

internal class MarkdownService
{
    public void BuildDocumentation(CommandLineArgs args)
    {
        var indexOutput = new StringBuilder();
        indexOutput.AppendLine($"# Scripting API Reference\n");

        foreach (var xmlFile in FileService.GetXmlFiles(args.XmlFilesDirectory))
        {
            this.BuildWithIndex(xmlFile, args.AssemblyDirectory, args.OutputDirectory, indexOutput, args.Template, args.Filter);
        }

        var outputFile = Path.GetFullPath(args.OutputDirectory + "/Index.md");
        FileService.WriteToFile(outputFile, indexOutput.ToString());
    }

    private void BuildWithIndex(string xmlFile, string assemblyDirectory, string outputDirectory, StringBuilder indexBuilder, string templateFile = null, string filter = null)
    {
        var xml = XDocument.Load(xmlFile);
        var root = xml.Root;
        var membersContainer = root.Element("members");
        var xmlMembers = membersContainer.Elements("member");
        outputDirectory = !outputDirectory.EndsWith("\\") ? $"{outputDirectory}\\" : outputDirectory;

        SortedDictionary<MarkdownMeta, List<Member>> outputParents = new();

        var assemblyName = root.Element("assembly").Element("name").Value;
        AssemblyHelper.LoadWithRelated(assemblyDirectory, assemblyName, out var isPlugin);

        this.ProcessXmlMembers(xmlMembers, outputParents, isPlugin, filter, assemblyDirectory, outputDirectory, templateFile);

        this.WriteOutputParentToIndex(outputParents, indexBuilder);
    }

    private void ProcessXmlMembers(IEnumerable<XElement> xmlMembers, SortedDictionary<MarkdownMeta, List<Member>> outputParents, bool isPlugin, string filter, string assemblyDirectory, string outputDirectory, string templateFile)
    {
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
            if (filter != null && xmlMember.XPathSelectElement(filter) == null)
            {
                continue;
            }

            var member = (FunctionMember)FunctionMember.FromXml(xmlMember, isPlugin);

            if (member.Parameters != null)
            {
                this.ProcessEnumMembers(member, enumerations, enumParent, outputDirectory, xmlMembers);
            }

            if (!outputParents.TryGetValue(member.Parent, out var parentList))
            {
                parentList = new List<Member>();

                outputParents.Add(member.Parent, parentList);
            }
            parentList.Add(member);

            this.ProcessFunctionMember(member, outputDirectory, templateFile);
        }
    }

    private void ProcessEnumMembers(FunctionMember member, List<Member> enumerations, MarkdownMeta enumParent, string outputDirectory, IEnumerable<XElement> xmlMembers)
    {
        foreach (var parameter in member.Parameters.Where(p => p.Type.IsEnum))
        {
            var enumName = parameter.Type.Name;
            var isTypeDescribed = enumerations.Exists(m =>
            {
                var enumMember = (EnumMember)m;
                return enumMember.Name == enumName;
            });

            if (!isTypeDescribed)
            {
                var enumMember = this.CreateEnumMember(parameter, enumParent, xmlMembers);
                enumerations.Add(enumMember);

                var outputEnumFile = Path.GetFullPath(outputDirectory + enumMember.Parent.Path + enumMember.Name.FirstCharToUpper() + ".md");
                var enumTemplateFile = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "DefaultEnumTemplate.md"));
                var enumTemplateContent = FileService.ReadFile(enumTemplateFile);

                FileService.WriteToFile(outputEnumFile, enumMember.FillTemplate(enumTemplateContent));
            }
        }
    }

    private EnumMember CreateEnumMember(Parameter parameter, MarkdownMeta enumParent, IEnumerable<XElement> xmlMembers)
    {
        Dictionary<string, string> valueSummaries = new();
        var fullXmlTypeName = $"T:{parameter.Type.FullName.Replace('+', '.')}";
        var fullXmlName = $"F:{parameter.Type.FullName.Replace('+', '.')}";
        var summary = string.Empty;

        foreach (var m in xmlMembers)
        {
            var name = m.Attribute("name").Value;

            if (name.Equals(fullXmlTypeName))
            {
                summary = m.Element("summary").Value.TrimEachLine();
            }
            else if (name.StartsWith(fullXmlName))
            {
                var valueSummary = m.Element("summary").Value.TrimEachLine().Trim('\n');
                valueSummaries.Add(name.Substring(fullXmlName.Length + 1), valueSummary);
            }
        }

        return new EnumMember()
        {
            Parent = enumParent,
            Type = parameter.Type,
            Name = parameter.Type.Name,
            Summary = summary,
            ValueSummaries = valueSummaries,
        };
    }

    private void ProcessFunctionMember(FunctionMember member, string outputDirectory, string templateFile)
    {
        var outputMemberFile = Path.GetFullPath(outputDirectory + member.Parent.Path + member.Name.FirstCharToUpper() + ".md");
        templateFile ??= Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "DefaultTemplate.md"));

        var templateContent = FileService.ReadFile(templateFile);
        FileService.WriteToFile(outputMemberFile, member.FillTemplate(templateContent));
    }

    private void WriteOutputParentToIndex(SortedDictionary<MarkdownMeta, List<Member>> outputParents, StringBuilder indexBuilder)
    {
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
