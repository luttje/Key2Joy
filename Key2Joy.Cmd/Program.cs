using Key2Joy.Config;
using Key2Joy.Mapping;
using Key2Joy.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using System.Reflection;

namespace Key2Joy.Cmd
{
    internal class Program
    {
        static void Main(string[] args)
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
