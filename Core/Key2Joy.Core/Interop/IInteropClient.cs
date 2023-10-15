namespace Key2Joy.Interop;

public interface IInteropClient
{
    void SendCommand<TCommandType>(TCommandType command);
}
