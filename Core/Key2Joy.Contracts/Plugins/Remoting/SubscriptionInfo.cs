using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
