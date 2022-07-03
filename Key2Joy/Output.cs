using System;
using System.Collections.Generic;
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

        internal static OutputModes OutputMode { get; set; } = OutputModes.Verbose;

        internal static void WriteLine(params object[] parts)
        {
            var output = new StringBuilder();
            output.Append("[");
            output.Append(DateTime.Now.ToString("HH:mm:ss"));
            output.Append("] ");

            foreach (var part in parts)
                output.Append(part.ToString());

            System.Diagnostics.Debug.WriteLine(output.ToString());

            var directory = Config.Instance.LogOutputPath;

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var file = Path.Combine(directory, $"{DateTime.Now.ToString("yyyy-MM-dd")}.log");
            
            File.AppendAllText(file, output.ToString() + Environment.NewLine);
        }

        internal static void WriteLine(OutputModes messageMode = OutputModes.Default, params object[] parts)
        {
            if(OutputMode != OutputModes.Verbose && messageMode == OutputModes.Verbose)
                return;

            WriteLine(parts);
        }
    }
}
