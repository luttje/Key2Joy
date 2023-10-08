using System;
using System.Runtime.Serialization;

namespace Key2Joy.Contracts.Plugins.Remoting;

[Serializable]
public struct SubscriptionInfo : ISafeSerializationData
{
    public string EventName { get; set; }
    public string Id { get; set; }

    public SubscriptionInfo(string eventName, string subscriptionId)
    {
        this.EventName = eventName ?? throw new ArgumentNullException(nameof(eventName));
        this.Id = subscriptionId;
    }

    public readonly void CompleteDeserialization(object deserialized)
    {
    }
}
