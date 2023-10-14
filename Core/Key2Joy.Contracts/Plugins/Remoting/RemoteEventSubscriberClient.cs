using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace Key2Joy.Contracts.Plugins.Remoting;

public class RemoteEventSubscriberClient
{
    public event EventHandler Disposing;

    private readonly NamedPipeClientStream pipeStream;

    private DateTime lastHeartbeatAt = DateTime.Now;
    private CancellationTokenSource pipeCancellation;

    internal RemoteEventSubscriberClient(string portName)
    {
        this.pipeStream = new NamedPipeClientStream(
            ".",
            RemotePipe.GetClientPipeName(portName),
            PipeDirection.InOut,
            PipeOptions.Asynchronous);
        this.pipeStream.Connect();
        this.pipeStream.ReadMode = PipeTransmissionMode.Message;

        this.pipeCancellation = new CancellationTokenSource();

        this.InitBackgroundHeartbeatThread();
    }

    /// <summary>
    /// Called from the client to send a message to the host.
    /// </summary>
    /// <param name="message"></param>
    /// <exception cref="IOException"></exception>
    internal void SendToHost(string message) => RemotePipe.WriteMessage(this.pipeStream, message);

    public void AskHostToInvokeSubscription(RemoteEventArgs e) => this.SendToHost(e.Subscription.Id.ToString());

    /// <summary>
    /// Checks in the background if the host has shown any sign of life recently.
    /// </summary>
    private void InitBackgroundHeartbeatThread()
    {
        // Registers heartbeats from the host.
        var pipeListenerBackgroundThread = Task.Run(() =>
        {
            try
            {
                while (!this.pipeCancellation.IsCancellationRequested)
                {
                    var message = RemotePipe.ReadMessage(this.pipeStream);

                    if (string.IsNullOrEmpty(message))
                    {
                        continue;
                    }

                    Console.WriteLine($"Remote Message: {message}");
                    this.lastHeartbeatAt = DateTime.Now;
                }
            }
            catch (ObjectDisposedException) { } // in case pipe closed
        });

        // Checks if the last heartbeat was too long ago.
        var pipeCancellerBackgroundThread = Task.Run(() =>
        {
            while (!this.pipeCancellation.IsCancellationRequested)
            {
                if (DateTime.Now - this.lastHeartbeatAt > RemoteEventSubscriber.MaxHeartbeatInterval)
                {
                    Console.WriteLine("Host is not responding. Shutting down.");
                    this.Dispose();
                    return;
                }

                Thread.Sleep(1000);
            }
        });
    }

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
            Output.WriteLine(ex);
            Debug.WriteLine(ex);
        }

        this.Dispose();
    }

    public void Dispose()
    {
        this.Disposing?.Invoke(this, EventArgs.Empty);

        this.pipeStream?.Dispose();

        this.pipeCancellation?.Cancel();
    }
}
