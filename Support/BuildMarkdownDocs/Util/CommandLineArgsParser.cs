using System;

namespace BuildMarkdownDocs.Util;

public static class CommandLineArgsParser
{
    public static CommandLineArgs Parse(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: BuildMarkdownDocs.exe <directory containing all referenced assemblies> <directory containing xml documentation files> <output directory> --template [template] --filter [xpath filter]");
            return null;
        }

        var commandLineArgs = new CommandLineArgs
        {
            AssemblyDirectory = args[0],
            XmlFilesDirectory = args[1],
            OutputDirectory = args[2]
        };

        for (var i = 2; i < args.Length; i++)
        {
            if (args[i] == "--template")
            {
                commandLineArgs.Template = args[i + 1];
                i++;
            }
            else if (args[i] == "--filter")
            {
                commandLineArgs.Filter = args[i + 1];
                i++;
            }
        }

        return commandLineArgs;
    }
}
