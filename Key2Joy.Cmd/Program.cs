using CommandLine;
using System;
using System.Linq;
using System.Reflection;

namespace Key2Joy.Cmd
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var types = LoadVerbs();

            Parser.Default.ParseArguments(args, types)
                  .WithParsed(Run);
        }

        private static Type[] LoadVerbs()
        {
            return Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetCustomAttribute<VerbAttribute>() != null).ToArray();
        }

        private static void Run(object obj)
        {
            if (obj is Options options)
            {
                options.Handle();
            }
            else
            {
                Console.WriteLine("Unknown command");
            }
        }
    }
}
