using System;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using Key2Joy.Interop.Commands;
using Key2Joy.Mapping;

namespace Key2Joy.Interop;

public class InteropServer : IInteropServer, IDisposable
{
    public const string PIPE_NAME = "Key2JoyService";

    private readonly IKey2JoyManager manager;
    private readonly ICommandRepository commandRepository;

    private NamedPipeServerStream pipeServer;

    public InteropServer(IKey2JoyManager manager, ICommandRepository commandRepository)
    {
        this.manager = manager ?? throw new ArgumentNullException(nameof(manager));
        this.commandRepository = commandRepository ?? throw new ArgumentNullException(nameof(commandRepository));
    }

    public void Dispose() => this.StopListening();

    public void RestartListening()
    {
        this.pipeServer?.Dispose();

        this.pipeServer = new NamedPipeServerStream(PIPE_NAME, PipeDirection.InOut, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
        this.pipeServer.BeginWaitForConnection(this.OnClientConnected, this.pipeServer);
    }

    public void StopListening() => this.pipeServer.Dispose();

    private void HandleEnableCommand(EnableCommand command)
        => this.manager.CallOnUiThread(()
        =>
        {
            var profile = MappingProfile.Load(command.ProfilePath);
            this.manager.ArmMappings(profile);
        });

    private void HandleDisableCommand(DisableCommand command)
        => this.manager.CallOnUiThread(()
        =>
        {
            if (this.manager.GetIsArmed())
            {
                this.manager.DisarmMappings();
            }
        });

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

        var commandId = ReadCommandId(pipeServer);
        var commandInfo = this.GetCommandInfo(commandId);

        if (commandInfo != null)
        {
            var command = ReadCommand(pipeServer, commandInfo);
            this.HandleCommand(command);
        }

        this.RestartListening();
    }

    /// <summary>
    /// Read the first byte and use it to get the type struct
    /// </summary>
    /// <param name="pipeServer"></param>
    /// <returns></returns>
    public static byte ReadCommandId(NamedPipeServerStream pipeServer)
        => (byte)pipeServer.ReadByte();

    /// <summary>
    /// Convert a byte identifier to command info by looking it up
    /// </summary>
    /// <param name="commandId"></param>
    /// <returns></returns>
    public CommandInfo GetCommandInfo(byte commandId)
        => this.commandRepository.GetCommandInfo(commandId);

    /// <summary>
    /// Reads the full command of a certain type from the pipe
    /// </summary>
    /// <param name="pipeServer"></param>
    /// <param name="commandInfo"></param>
    /// <returns></returns>
    public static object ReadCommand(NamedPipeServerStream pipeServer, CommandInfo commandInfo)
    {
        var bytes = new byte[Marshal.SizeOf(commandInfo.StructType)];
        pipeServer.Read(bytes, 0, bytes.Length);
        return commandInfo.CommandFromBytes(bytes);
    }

    /// <summary>
    /// Handle a command by calling the appropriate handler
    /// </summary>
    /// <param name="command"></param>
    public void HandleCommand(object command)
    {
        switch (command)
        {
            case EnableCommand enableCommand:
                this.HandleEnableCommand(enableCommand);
                break;

            case DisableCommand disableCommand:
                this.HandleDisableCommand(disableCommand);
                break;

            default:
                break;
        }
    }
}
