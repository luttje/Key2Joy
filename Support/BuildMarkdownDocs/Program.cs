using BuildMarkdownDocs.Util;

namespace BuildMarkdownDocs;

internal class Program
{
    private static void Main(string[] args)
    {
        var commandLineArgs = CommandLineArgsParser.Parse(args);

        if (commandLineArgs == null)
        {
            return;
        }

        var markdownService = new MarkdownService();
        markdownService.BuildDocumentation(commandLineArgs);
    }
}
