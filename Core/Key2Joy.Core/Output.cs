using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Key2Joy.Config;

namespace Key2Joy;

public static class Output
{
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

    public static string GetLogPath(DateTime? day = null)
    {
        if (day == null)
        {
            day = DateTime.Now;
        }

        var directory = ConfigManager.Config.LogOutputPath;

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
        File.AppendAllText(GetLogPath(), outputLine + Environment.NewLine);

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
