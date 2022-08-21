using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy
{
    internal static class Output
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

        internal static event Action<string> OnNewLogLine;

        internal static OutputModes OutputMode { get; set; } = OutputModes.Verbose;

        internal static string GetLogPath(DateTime? day = null)
        {
            if (day == null)
                day = DateTime.Now;

            var directory = Config.Instance.LogOutputPath;

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var dayString = ((DateTime)day).ToString("yyyy-MM-dd");
            return Path.Combine(directory, $"{dayString}.log");
        }

        internal static void WriteLine(params object[] parts)
        {
            var output = new StringBuilder();
            output.Append("[");
            output.Append(DateTime.Now.ToString("HH:mm:ss"));
            output.Append("] ");

            foreach (var part in parts)
                output.Append(part.ToString());

            var outputLine = output.ToString();

            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "Application";
                eventLog.WriteEntry(outputLine, EventLogEntryType.Information, 101, 1);
            }

            Debug.WriteLine(outputLine);
            File.AppendAllText(GetLogPath(), outputLine + Environment.NewLine);

            OnNewLogLine?.Invoke(outputLine);
        }

        internal static void WriteLine(OutputModes messageMode = OutputModes.Default, params object[] parts)
        {
            if(OutputMode != OutputModes.Verbose && messageMode == OutputModes.Verbose)
                return;

            WriteLine(parts);
        }
    }
}
