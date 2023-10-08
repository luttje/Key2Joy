using System;

namespace BuildMarkdownDocs
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: BuildMarkdownDocs.exe <xml doc file> <output directory> --template [template] --filter [xpath filter]");
                return;
            }

            var xmlFile = args[0];
            var outputDirectory = args[1];
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

            // TODO: This is hacky, but it works. We're making sure to have a reference to the assembly, so it's loaded.
            TypeUtil.NotifyAssemblyRelation(typeof(Key2Joy.Key2JoyManager));

            MarkdownDocs.Build(xmlFile, outputDirectory, template, filter);
        }
    }
}
