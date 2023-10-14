using System;
using System.IO;
using System.Text;
using BuildMarkdownDocs.Util;

namespace BuildMarkdownDocs;

internal class Program
{
    private static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: BuildMarkdownDocs.exe <directory containing all referenced assemblies> <directory containing xml documentation files> <output directory> --template [template] --filter [xpath filter]");
            return;
        }

        var assemblyDirectory = args[0];
        var xmlFilesDirectory = args[1];
        var outputDirectory = args[2];
        string template = null;
        string filter = null;

        for (var i = 2; i < args.Length; i++)
        {
            if (args[i] == "--template")
            {
                template = args[i + 1];
                i++;
            }
            else if (args[i] == "--filter")
            {
                filter = args[i + 1];
                i++;
            }
        }

        var outputFile = Path.GetFullPath(outputDirectory + "/Index.md");

        StringBuilder indexOutput = new();
        indexOutput.AppendLine($"# Scripting API Reference\n");

        foreach (var xmlFile in Directory.GetFiles(xmlFilesDirectory, "*.xml"))
        {
            MarkdownDocs.BuildWithIndex(xmlFile, assemblyDirectory, outputDirectory, indexOutput, template, filter);
        }

        FileHelper.WriteToFile(outputFile, indexOutput.ToString());
    }
}
