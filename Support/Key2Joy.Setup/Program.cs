using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using CommandLine;
using Key2Joy.Setup.Cmd;

namespace Key2Joy.Setup;

internal static class Program
{
    internal static bool ShouldStart { get; set; } = true;
    internal static bool IsElevated => System.Security.Principal.WindowsIdentity.GetCurrent().Owner
                .IsWellKnown(System.Security.Principal.WellKnownSidType.BuiltinAdministratorsSid);

    internal static bool Elevate(string argumentString)
    {
        ProcessStartInfo processInfo = new(Assembly.GetExecutingAssembly().CodeBase)
        {
            Verb = "runas"
        };

        try
        {
            processInfo.Arguments = argumentString;
            Process.Start(processInfo);
            //Application.Exit();
            return true;
        }
        catch (System.ComponentModel.Win32Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        var args = Environment.GetCommandLineArgs().Skip(1).ToArray();
        var types = LoadVerbs();

        if (args.Length > 0)
        {
            Parser.Default.ParseArguments(args, types)
                  .WithParsed(Run)
                  .WithNotParsed(HandleErrors);
        }

        if (!ShouldStart)
        {
            return;
        }

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new SetupForm());
    }

    private static void HandleErrors(IEnumerable<Error> obj) => MessageBox.Show("Error parsing arguments. Please check the command line arguments.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

    private static void Run(object obj)
    {
        if (obj is Options options)
        {
            options.Handle();
        }
        else
        {
            MessageBox.Show("Unknown command specified on CLI");
        }
    }

    private static Type[] LoadVerbs() => Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.GetCustomAttribute<VerbAttribute>() != null).ToArray();
}
