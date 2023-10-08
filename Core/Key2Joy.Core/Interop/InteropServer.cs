using Key2Joy.Mapping;
using System;
using System.IO.Pipes;
using System.Runtime.InteropServices;

namespace Key2Joy.Interop
{
    internal class InteropServer : IDisposable
    {
        public const string PIPE_NAME = "Key2JoyService";

        public static InteropServer Instance { get; } = new InteropServer();

        private NamedPipeServerStream pipeServer;

        private InteropServer()
        { }

        public void Dispose()
        {
            StopListening();
        }

        public void RestartListening()
        {
            pipeServer?.Dispose();

            pipeServer = new NamedPipeServerStream(PIPE_NAME, PipeDirection.InOut, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
            pipeServer.BeginWaitForConnection(OnClientConnected, pipeServer);
        }

        public void StopListening()
        {
            pipeServer.Dispose();
        }

        private void HandleEnableCommand(EnableCommand command)
        {
            Key2JoyManager.Instance.CallOnUiThread(() =>
            {
                var profile = MappingProfile.Load(command.ProfilePath);
                Key2JoyManager.Instance.ArmMappings(profile);
            });
        }

        private void HandleDisableCommand(DisableCommand command)
        {
            Key2JoyManager.Instance.CallOnUiThread(() =>
            {
                if (Key2JoyManager.Instance.GetIsArmed())
                {
                    Key2JoyManager.Instance.DisarmMappings();
                }
            });
        }

        private void OnClientConnected(IAsyncResult asyncResult)
        {
            var pipeServer = (NamedPipeServerStream)asyncResult.AsyncState;

            try
            {
                pipeServer.EndWaitForConnection(asyncResult);
            }
            catch (ObjectDisposedException)
            {
                // Ignore when pipe is closed
                return;
            }

            // Read the first byte and use it to get the type struct
            var commandId = (byte)pipeServer.ReadByte();
            var commandInfo = CommandRepository.Instance.GetCommandInfo(commandId);

            // Marshall the other bytes to the given struct
            var bytes = new byte[Marshal.SizeOf(commandInfo.StructType)];
            pipeServer.Read(bytes, 0, bytes.Length);
            var command = commandInfo.CommandFromBytes(bytes);

            switch (command)
            {
                case EnableCommand enableCommand:
                    HandleEnableCommand(enableCommand);
                    break;
                case DisableCommand disableCommand:
                    HandleDisableCommand(disableCommand);
                    break;
            }

            // Restore the connection for the next client
            RestartListening();
        }
    }
}
