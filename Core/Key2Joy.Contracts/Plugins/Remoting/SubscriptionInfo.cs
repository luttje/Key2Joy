using System;
using System.Runtime.Serialization;

namespace Key2Joy.Contracts.Plugins
{
    [Serializable]
    public struct SubscriptionInfo : ISafeSerializationData
    {
        public string EventName { get; set; }
        public string Id { get; set; }

        public SubscriptionInfo(string eventName, string subscriptionId)
        {
            EventName = eventName ?? throw new ArgumentNullException(nameof(eventName));
            Id = subscriptionId;
        }

        public void CompleteDeserialization(object deserialized)
        {
        }
    }
}
