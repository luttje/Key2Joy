using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Key2Joy.Contracts.Util;

namespace Key2Joy.Contracts;

public static class Output
{
    private const string APP_DIR = "Key2Joy";
    private const string LogOutputPath = "Logs";

    public enum OutputModes
    {
        /// <summary>
        /// Outputs prints, warnings and error messages
        /// </summary>
        Default,

        /// <summary>
        /// Prints everything
        /// </summary>
        Verbose
    }

    public static event Action<string> OnNewLogLine;

    public static OutputModes OutputMode { get; set; } = OutputModes.Verbose;

    public static string GetAppDataDirectory()
    {
        var directory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            APP_DIR);

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        return directory;
    }

    public static string GetLogPath(DateTime? day = null)
    {
        if (day == null)
        {
            day = DateTime.Now;
        }

        var directory = Path.Combine(GetAppDataDirectory(), LogOutputPath);

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var dayString = ((DateTime)day).ToString("yyyy-MM-dd");
        return Path.Combine(directory, $"{dayString}.log");
    }

    public static void WriteLine(params object[] parts)
    {
        StringBuilder output = new();
        output.Append("[");
        output.Append(DateTime.Now.ToString("HH:mm:ss"));
        output.Append("] ");

        foreach (var part in parts)
        {
            output.Append(part.ToString());
        }

        var outputLine = output.ToString();

        using (EventLog eventLog = new("Application"))
        {
            eventLog.Source = "Application";
            eventLog.WriteEntry(outputLine, EventLogEntryType.Information, 101, 1);
        }

        Debug.WriteLine(outputLine);
        RetryHelper.RetryOnException(
            () => File.AppendAllText(GetLogPath(), outputLine + Environment.NewLine),
            TimeSpan.FromSeconds(1),
            5,
            typeof(IOException)
        );

        OnNewLogLine?.Invoke(outputLine);
    }

    public static void WriteLine(OutputModes messageMode = OutputModes.Default, params object[] parts)
    {
        if (OutputMode != OutputModes.Verbose && messageMode == OutputModes.Verbose)
        {
            return;
        }

        WriteLine(parts);
    }
}
