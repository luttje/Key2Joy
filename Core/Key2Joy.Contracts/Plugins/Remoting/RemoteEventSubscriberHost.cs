using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;
using System.Threading;

namespace Key2Joy.Contracts.Plugins.Remoting;

public class RemoteEventSubscriberHost : IDisposable
{
    public event EventHandler Disposing;

    private readonly NamedPipeServerStream pipeStream;
    private bool isPipeServerStreamReady = false;

    internal RemoteEventSubscriberHost(NamedPipeServerStream pipeStream)
    {
        this.pipeStream = pipeStream;

        this.InitEventThread();
    }

    /// <summary>
    /// Exposes a named pipe endpoint corresponding to the unique name for this plugin host
    /// </summary>
    private void InitEventThread()
    {
        // Start a new thread to listen for incoming connections on the named pipe
        Thread eventPipeThread = null;
        eventPipeThread = new(() =>
        {
            this.pipeStream.WaitForConnection();
            Debug.WriteLine($"Pipe connection with plugin established");

            StreamReader reader = new(this.pipeStream);

            while (true)
            {
                try
                {
                    var messageOrSubscriptionId = RemotePipe.ReadMessage(this.pipeStream);

                    if (string.IsNullOrEmpty(messageOrSubscriptionId))
                    {
                        continue;
                    }

                    Debug.WriteLine($"ReadMessage: {messageOrSubscriptionId}");

                    if (messageOrSubscriptionId == RemoteEventSubscriber.SignalReady)
                    {
                        this.isPipeServerStreamReady = true;
                        continue;
                    }
                    else if (messageOrSubscriptionId == RemoteEventSubscriber.SignalExit)
                    {
                        this.Dispose();

                        // End and dispose this thread.
                        eventPipeThread.Abort();
                        eventPipeThread = null;
                        continue;
                    }

                    RemoteEventSubscriber.HandleInvoke(messageOrSubscriptionId);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"-------------> Exception: {ex.Message}");
                }
            }
        })
        {
            IsBackground = true
        };
        eventPipeThread.Start();
    }

    /// <summary>
    /// Stops execution until the event pipe has received the ready signal
    /// </summary>
    public void WaitForEventPipeReady()
    {
        while (!this.isPipeServerStreamReady)
        {
            Task.Delay(10);
        }
    }

    public void Dispose()
    {
        this.Disposing?.Invoke(this, EventArgs.Empty);
        this.pipeStream?.Dispose();
    }
}
