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
using System.Diagnostics;
using System.Threading;

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

        internal static void StartKey2Joy()
        {
            var executablePath = ConfigManager.Instance.LastInstallPath;

            if (executablePath == null)
            {
                Console.WriteLine("Error! Key2Joy executable path is not known, please start Key2Joy at least once!");
                return;
            }

            if (!System.IO.File.Exists(executablePath))
            {
                Console.WriteLine("Error! Key2Joy executable path is invalid, please start Key2Joy at least once (and don't move the executable)!");
                return;
            }
            
            var process = Process.Start(executablePath);

            // Pause while we wait for the process to start
            while (string.IsNullOrEmpty(process.MainWindowTitle))
            {
                Thread.Sleep(100);
                process.Refresh();
            }
        }
    }
}
