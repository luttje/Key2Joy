using Key2Joy.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Key2Joy.Interop
{
    public class InteropClient
    {
        public static InteropClient Instance { get; } = new InteropClient();

        private InteropClient()
        { }

        public void SendCommand<CommandType>(CommandType command)
        {
            Console.WriteLine("Sending command");
            using var pipeClient = new NamedPipeClientStream(".", InteropServer.PIPE_NAME,
                        PipeDirection.InOut, PipeOptions.None);
            pipeClient.Connect(1000);

            var commandInfo = CommandRepository.Instance.GetCommandInfo(command);

            pipeClient.WriteByte(commandInfo.Id);
            Console.WriteLine("Command ID: " + commandInfo.Id);

            var bytes = commandInfo.CommandToBytes(command);
            pipeClient.Write(bytes, 0, bytes.Length);
            Console.WriteLine("Command bytes: " + bytes.Length);
            foreach (var b in bytes)
            {
                Console.Write(b.ToString() + " ");
            }

            pipeClient.WaitForPipeDrain();
        }
    }
}
