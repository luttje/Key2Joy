using System;
using System.Linq;
using System.Reflection;
using CommandLine;
using Key2Joy.Interop;
using Key2Joy.Interop.Commands;

namespace Key2Joy.Cmd;

internal class Program
{
    private static void Main(string[] args)
    {
        var types = LoadVerbs();

        Parser.Default.ParseArguments(args, types)
              .WithParsed(Run);
    }

    private static Type[] LoadVerbs() => Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.GetCustomAttribute<VerbAttribute>() != null).ToArray();

    private static void Run(object obj)
    {
        if (obj is Options options)
        {
            options.Handle(
                new InteropClient(
                    new CommandRepository()
                )
            );
        }
        else
        {
            Console.WriteLine("Unknown command");
        }
    }
}
