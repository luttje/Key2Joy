using System.IO;
using System.IO.Pipes;
using System.Text;

namespace Key2Joy.Contracts.Plugins.Remoting;

public class RemotePipe
{
    private const string Prefix = "EventPipe.";
    internal const string PipeNameFormatHost = $@"\\.\{Prefix}{{0}}";
    internal const string PipeNameFormatClient = $@"{Prefix}{{0}}";

    /// <summary>
    /// For some reason the host needs this to setup. The client works without since it manually provides the host.
    /// </summary>
    /// <param name="name"></param>
    /// <returns>The absolute pipe name</returns>
    public static string GetAbsolutePipeName(string name) => string.Format(PipeNameFormatHost, name);

    /// <summary>
    /// The pipe name without the host.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string GetClientPipeName(string name) => string.Format(PipeNameFormatClient, name);

    public static string ReadMessage(PipeStream pipe)
    {
        var buffer = new byte[1024];
        using MemoryStream ms = new();

        do
        {
            var readBytes = pipe.Read(buffer, 0, buffer.Length);
            ms.Write(buffer, 0, readBytes);
        }
        while (!pipe.IsMessageComplete);

        return Encoding.UTF8.GetString(ms.ToArray());
    }

    internal static void WriteMessage(PipeStream pipeStream, string message)
    {
        var buffer = Encoding.UTF8.GetBytes(message);
        pipeStream.Write(buffer, 0, buffer.Length);
        pipeStream.WaitForPipeDrain();
    }
}
