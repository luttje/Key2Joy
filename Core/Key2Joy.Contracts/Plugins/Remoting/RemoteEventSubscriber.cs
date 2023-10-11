using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Text;

namespace Key2Joy.Contracts.Plugins.Remoting;

public class FullSubscriptionInfo
{
    public SubscriptionInfo Subscription { get; private set; }
    public RemoteEventHandlerCallback EventHandler { get; private set; }
    public object CustomSender { get; set; }

    public FullSubscriptionInfo(string eventName, string subscriptionId, RemoteEventHandlerCallback eventHandler, object customSender = null)
    {
        this.Subscription = new SubscriptionInfo(eventName, subscriptionId);
        this.EventHandler = eventHandler;
        this.CustomSender = customSender;
    }
}

public class RemoteEventSubscriber : MarshalByRefObject
{
    public const string SignalReady = "READY";
    public const string SignalExit = "EXIT";

    private static readonly Dictionary<string, FullSubscriptionInfo> Subscriptions = new();

    public static RemoteEventSubscriber ClientInstance { get; private set; }
    private readonly NamedPipeClientStream pipeClientStream;

    private RemoteEventSubscriber(NamedPipeClientStream pipeClientStream) => this.pipeClientStream = pipeClientStream;

    private void SendToServer(string message)
    {
        var buffer = Encoding.UTF8.GetBytes(message);

        this.pipeClientStream.Write(buffer, 0, buffer.Length);
        this.pipeClientStream.WaitForPipeDrain();
    }

    public static void InitClient(NamedPipeClientStream pipeClientStream)
    {
        if (!pipeClientStream.IsConnected)
        {
            throw new ArgumentException($"The pipeClientStream must be connected before calling {nameof(InitClient)}.");
        }

        ClientInstance = new RemoteEventSubscriber(pipeClientStream);
        ClientInstance.SendToServer(SignalReady);
    }

    public static void ExitClient()
    {
        if (!ClientInstance.pipeClientStream.IsConnected)
        {
            return;
        }

        ClientInstance.SendToServer(SignalExit);
    }

    public static FullSubscriptionInfo SubscribeEvent(string eventName, RemoteEventHandlerCallback handler)
    {
        var subscriptionId = Guid.NewGuid().ToString();
        FullSubscriptionInfo subscription = new(eventName, subscriptionId, handler);

        Subscriptions.Add(subscriptionId, subscription);

        return subscription;
    }

    public static void UnsubscribeEvent(string subscriptionId) => Subscriptions.Remove(subscriptionId);

    public static bool TryGetSubscription(string subscriptionId, out FullSubscriptionInfo subscription) => Subscriptions.TryGetValue(subscriptionId, out subscription);

    public void AskServerToInvokeSubscription(RemoteEventArgs e)
    {
        SendToServer(e.Subscription.Id.ToString());
    }

    public static void HandleInvoke(string subscriptionId)
    {
        // Find subscription and call related event
        if (!TryGetSubscription(subscriptionId, out var fullSubscriptionInfo))
        {
            throw new ApplicationException($"Could not find event subscription with id {subscriptionId}!");
        }

        fullSubscriptionInfo.EventHandler?.Invoke(fullSubscriptionInfo.CustomSender, new RemoteEventArgs(fullSubscriptionInfo.Subscription));
    }
}
