using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text;

namespace Key2Joy.Contracts.Plugins.Remoting;

public class RemoteEventSubscriber : MarshalByRefObject
{
    public const string SignalReady = "READY";
    public const string SignalExit = "EXIT";

    private static readonly Dictionary<string, SubscriptionRegistration> Subscriptions = new();

    public static RemoteEventSubscriber ClientInstance { get; private set; }
    private readonly NamedPipeClientStream pipeClientStream;

    private RemoteEventSubscriber(NamedPipeClientStream pipeClientStream) => this.pipeClientStream = pipeClientStream;

    /// <summary>
    /// Called from the client to send a message to the host.
    /// </summary>
    /// <param name="message"></param>
    /// <exception cref="IOException"></exception>
    private void SendToHost(string message)
    {
        var buffer = Encoding.UTF8.GetBytes(message);

        try
        {
            this.pipeClientStream.Write(buffer, 0, buffer.Length);
            this.pipeClientStream.WaitForPipeDrain();
        }
        catch (IOException ex)
        {
            throw ex;
        }
    }

    public static void InitClient(NamedPipeClientStream pipeClientStream)
    {
        if (!pipeClientStream.IsConnected)
        {
            throw new ArgumentException($"The pipeClientStream must be connected before calling {nameof(InitClient)}.");
        }

        ClientInstance = new RemoteEventSubscriber(pipeClientStream);

        try
        {
            ClientInstance.SendToHost(SignalReady);
        }
        catch (IOException ex)
        {
            Debug.WriteLine(ex);
        }
    }

    public static void ExitClient()
    {
        if (!ClientInstance.pipeClientStream.IsConnected)
        {
            return;
        }

        try
        {
            ClientInstance.SendToHost(SignalExit);
        }
        catch (IOException ex)
        {
            Debug.WriteLine(ex);
        }
    }

    public static SubscriptionRegistration SubscribeEvent(string eventName, RemoteEventHandlerCallback handler)
    {
        var subscriptionId = Guid.NewGuid().ToString();
        SubscriptionRegistration subscription = new(eventName, subscriptionId, handler);

        Subscriptions.Add(subscriptionId, subscription);

        return subscription;
    }

    public static void UnsubscribeEvent(string subscriptionId) => Subscriptions.Remove(subscriptionId);

    public static bool TryGetSubscription(string subscriptionId, out SubscriptionRegistration subscription) => Subscriptions.TryGetValue(subscriptionId, out subscription);

    public void AskHostToInvokeSubscription(RemoteEventArgs e) => this.SendToHost(e.Subscription.Id.ToString());

    public static void HandleInvoke(string subscriptionId)
    {
        // Find subscription and call related event
        if (!TryGetSubscription(subscriptionId, out var fullSubscriptionInfo))
        {
            throw new ApplicationException($"Could not find event subscription with id {subscriptionId}!");
        }

        fullSubscriptionInfo.EventHandler?.Invoke(fullSubscriptionInfo.CustomSender, new RemoteEventArgs(fullSubscriptionInfo.Ticket));
    }
}
