using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text;

namespace Key2Joy.Contracts.Plugins.Remoting;

public class RemoteEventSubscriberClient
{
    public event EventHandler Disposing;

    private readonly NamedPipeClientStream pipeStream;

    internal RemoteEventSubscriberClient(NamedPipeClientStream pipeStream) => this.pipeStream = pipeStream;

    public void Exit()
    {
        if (!this.pipeStream.IsConnected)
        {
            return;
        }

        try
        {
            this.SendToHost(RemoteEventSubscriber.SignalExit);
        }
        catch (IOException ex)
        {
            Debug.WriteLine(ex);
        }

        this.Dispose();
    }

    /// <summary>
    /// Called from the client to send a message to the host.
    /// </summary>
    /// <param name="message"></param>
    /// <exception cref="IOException"></exception>
    internal void SendToHost(string message)
    {
        var buffer = Encoding.UTF8.GetBytes(message);

        try
        {
            this.pipeStream.Write(buffer, 0, buffer.Length);
            this.pipeStream.WaitForPipeDrain();
        }
        catch (IOException ex)
        {
            throw ex;
        }
    }

    public void AskHostToInvokeSubscription(RemoteEventArgs e) => this.SendToHost(e.Subscription.Id.ToString());

    public void Dispose()
    {
        this.Disposing?.Invoke(this, EventArgs.Empty);
        this.pipeStream?.Dispose();
    }
}
