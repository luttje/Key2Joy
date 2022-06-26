﻿using BuildMarkdownDocs.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            var directory = Path.GetDirectoryName(outputFile);

            foreach (var xmlMember in xmlMembers)
            {
                if(filter != null)
                {
                    var child = xmlMember.XPathSelectElement(filter);
                    
                    if (child == null)
                        continue;
                }

                var member = Member.FromXml(xmlMember);
                var outputMemberFile = Path.GetFullPath(outputDirectory + member.Parent.Path + member.Name.FirstCharToUpper() + ".md");
                var outputMember = new StringBuilder();

                if(!outputParents.TryGetValue(member.Parent, out var parent)) 
                { 
                    outputParents.Add(member.Parent, parent = new List<Member>());
                }

                parent.Add(member);

                Console.WriteLine($"Writing output to {outputMemberFile}");

                if (templateFile == null)
                    templateFile = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "DefaultTemplate.md"));
                
                Console.WriteLine($"Using template {templateFile}");

                var templateContent = File.ReadAllText(templateFile);
                outputMember.Append(member.FillTemplate(templateContent));

                var memberDirectory = Path.GetDirectoryName(outputMemberFile);

                if (!Directory.Exists(memberDirectory))
                    Directory.CreateDirectory(memberDirectory);

                File.WriteAllText(outputMemberFile, outputMember.ToString());
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
                    output.AppendLine($"* [`{member.Name}` ({member.GetParametersSignature()})]({member.Parent.Path}{member.Name}.md)");
                }
                output.AppendLine("");
            }

            File.WriteAllText(outputFile, output.ToString());
        }
    }
}
