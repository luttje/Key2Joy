using System;
using System.IO.Pipes;
using Key2Joy.Interop.Commands;

namespace Key2Joy.Interop;

/// <summary>
/// Singleton client for communication with the Key2Joy service.
/// This is used by the Key2Joy.Cmd CLI to send commands to the service and
/// enable/disable mappings.
/// </summary>
public class InteropClient : IInteropClient
{
    protected readonly ICommandRepository commandRepository;

    public InteropClient(ICommandRepository commandRepository)
        => this.commandRepository = commandRepository;

    /// <summary>
    /// Sends a command to the main app, for example to enable/disable mappings.
    /// </summary>
    /// <typeparam name="TCommandType"></typeparam>
    /// <param name="command"></param>
    public void SendCommand<TCommandType>(TCommandType command)
    {
        using NamedPipeClientStream pipeClient = new(".", this.GetPipeName(),
                    PipeDirection.InOut, PipeOptions.None);
        pipeClient.Connect(1000);

        var commandInfo = this.commandRepository.GetCommandInfo<TCommandType>();
        pipeClient.WriteByte(commandInfo.Id);

        var bytes = CommandInfo.CommandToBytes(command);
        pipeClient.Write(bytes, 0, bytes.Length);

        pipeClient.WaitForPipeDrain();
    }

    protected virtual string GetPipeName() => InteropServer.PIPE_NAME;
}
