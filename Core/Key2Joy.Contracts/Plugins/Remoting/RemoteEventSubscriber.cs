using System;
using System.Collections.Generic;
using System.IO;

namespace Key2Joy.Contracts.Plugins.Remoting;

public class RemoteEventSubscriber
{
    internal static readonly TimeSpan MaxHeartbeatInterval = TimeSpan.FromSeconds(10);

    public const string SignalReady = "READY";
    public const string SignalHeartbeat = "HEARTBEAT";
    public const string SignalExit = "EXIT";

    private static readonly Dictionary<string, SubscriptionRegistration> Subscriptions = new();

    public static RemoteEventSubscriberClient ClientInstance { get; private set; }

    /// <summary>
    /// Exposes a named pipe endpoint corresponding to the unique name for this plugin host
    /// </summary>
    public static RemoteEventSubscriberHost InitHostForPlugin(string portName) => new RemoteEventSubscriberHost(portName);

    public static void InitClient(string portName)
    {
        ClientInstance = new RemoteEventSubscriberClient(portName);

        try
        {
            ClientInstance.SendToHost(SignalReady);
        }
        catch (IOException ex)
        {
            Output.WriteLine(ex.ToString());
            Console.WriteLine(ex);
        }
    }

    public static SubscriptionRegistration SubscribeEvent(string eventName, RemoteEventHandlerCallback handler)
    {
        var subscriptionId = Guid.NewGuid().ToString();
        SubscriptionRegistration subscription = new(eventName, subscriptionId, handler);

        Subscriptions.Add(subscriptionId, subscription);

        return subscription;
    }

    public static void HandleInvoke(string subscriptionId)
    {
        // Find subscription and call related event
        if (!TryGetSubscription(subscriptionId, out var fullSubscriptionInfo))
        {
            throw new ApplicationException($"Could not find event subscription with id {subscriptionId}!");
        }

        fullSubscriptionInfo.EventHandler?.Invoke(fullSubscriptionInfo.CustomSender, new RemoteEventArgs(fullSubscriptionInfo.Ticket));
    }

    public static void UnsubscribeEvent(string subscriptionId) => Subscriptions.Remove(subscriptionId);

    public static bool TryGetSubscription(string subscriptionId, out SubscriptionRegistration subscription) => Subscriptions.TryGetValue(subscriptionId, out subscription);
}
