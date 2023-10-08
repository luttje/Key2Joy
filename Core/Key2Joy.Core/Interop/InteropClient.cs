using System.IO.Pipes;

namespace Key2Joy.Interop
{
    public class InteropClient
    {
        public static InteropClient Instance { get; } = new InteropClient();

        private InteropClient()
        { }

        public void SendCommand<CommandType>(CommandType command)
        {
            using var pipeClient = new NamedPipeClientStream(".", InteropServer.PIPE_NAME,
                        PipeDirection.InOut, PipeOptions.None);
            pipeClient.Connect(1000);

            var commandInfo = CommandRepository.Instance.GetCommandInfo(command);
            pipeClient.WriteByte(commandInfo.Id);

            var bytes = commandInfo.CommandToBytes(command);
            pipeClient.Write(bytes, 0, bytes.Length);

            pipeClient.WaitForPipeDrain();
        }
    }
}
