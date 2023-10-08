using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Contracts.Plugins
{
    public class RemotePipe
    {
        private const string Prefix = "EventPipe.";
        public const string PipeNameFormatServer = $@"\\.\{Prefix}{{0}}";
        public const string PipeNameFormatClient = $@"{Prefix}{{0}}";

        /// <summary>
        /// For some reason the server needs this to setup. The client works without since it manually provides the host.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetAbsolutePipeName(string name)
        {
            return string.Format(PipeNameFormatServer, name);
        }

        public static string GetClientPipeName(string name)
        {
            return string.Format(PipeNameFormatClient, name);
        }

        public static string ReadMessage(PipeStream pipe)
        {
            byte[] buffer = new byte[1024];
            using (var ms = new MemoryStream())
            {
                do
                {
                    var readBytes = pipe.Read(buffer, 0, buffer.Length);
                    ms.Write(buffer, 0, readBytes);
                }
                while (!pipe.IsMessageComplete);

                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }
}
