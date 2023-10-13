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

    private readonly CancellationTokenSource pipeCancellation;
    private static readonly TimeSpan HeartbeatWithMargin = RemoteEventSubscriber.MaxHeartbeatInterval - TimeSpan.FromSeconds(2);
    private static readonly TimeSpan MaxWaitForReady = TimeSpan.FromSeconds(5);

    internal RemoteEventSubscriberHost(string portName)
    {
        this.pipeStream = new NamedPipeServerStream(
            RemotePipe.GetAbsolutePipeName(portName),
            PipeDirection.InOut,
            1,
            PipeTransmissionMode.Message,
            PipeOptions.Asynchronous);

        this.pipeCancellation = new CancellationTokenSource();

        this.InitBackgroundEventThread();
        this.InitBackgroundHeartbeatThread();
    }

    /// <summary>
    /// Listens in the background for any incoming messages from the client.
    /// </summary>
    private async void InitBackgroundEventThread()
    {
        var backgroundThread = Task.Run(() =>
        {
            this.pipeStream.WaitForConnection();

            StreamReader reader = new(this.pipeStream);

            while (!this.pipeCancellation.IsCancellationRequested)
            {
                try
                {
                    var messageOrSubscriptionId = RemotePipe.ReadMessage(this.pipeStream);

                    if (string.IsNullOrEmpty(messageOrSubscriptionId))
                    {
                        continue;
                    }

                    if (messageOrSubscriptionId == RemoteEventSubscriber.SignalReady)
                    {
                        this.isPipeServerStreamReady = true;
                        continue;
                    }
                    else if (messageOrSubscriptionId == RemoteEventSubscriber.SignalExit)
                    {
                        this.Dispose();
                        return;
                    }

                    RemoteEventSubscriber.HandleInvoke(messageOrSubscriptionId);
                }
                catch (Exception ex)
                {
                    Output.WriteLine(ex);
                    Debug.WriteLine($"-------------> Exception: {ex.Message}");
                    return;
                }
            }
        });

        await backgroundThread;
    }

    /// <summary>
    /// Sends heartbeats in the background at the commonly agreed interval.
    /// </summary>
    private async void InitBackgroundHeartbeatThread()
    {
        var backgroundThread = Task.Run(async () =>
        {
            while (!this.pipeCancellation.IsCancellationRequested)
            {
                await Task.Delay(HeartbeatWithMargin);
                if (!this.isPipeServerStreamReady)
                {
                    continue;
                }

                try
                {
                    RemotePipe.WriteMessage(this.pipeStream, RemoteEventSubscriber.SignalHeartbeat);
                }
                catch (Exception ex)
                {
                    Output.WriteLine(ex);
                    Debug.WriteLine($"-------------> Exception: {ex.Message}");
                    return;
                }
            }
        });

        await backgroundThread;
    }

    /// <summary>
    /// Stops execution until the event pipe has received the ready signal
    /// </summary>
    public void WaitForEventPipeReady()
    {
        var waitStart = DateTime.Now;

        while (!this.isPipeServerStreamReady)
        {
            Task.Delay(10);

            if ((DateTime.Now - waitStart) > MaxWaitForReady)
            {
                throw new TimeoutException($"WaitForEventPipeReady timed out after {MaxWaitForReady.TotalMilliseconds}ms");
            }
        }
    }

    public void Dispose()
    {
        this.Disposing?.Invoke(this, EventArgs.Empty);

        this.pipeStream?.Dispose();

        this.pipeCancellation?.Cancel();
    }
}
