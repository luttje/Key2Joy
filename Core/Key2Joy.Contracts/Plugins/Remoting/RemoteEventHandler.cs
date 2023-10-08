using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Contracts.Plugins
{
    [Serializable]
    public class RemoteEventArgs : ISafeSerializationData
    {
        public SubscriptionInfo Subscription { get; set; }

        public RemoteEventArgs(SubscriptionInfo subscription)
        {
            Subscription = subscription;
        }

        public void CompleteDeserialization(object deserialized)
        { }
    }
    
    public delegate void RemoteEventHandlerCallback(object sender, RemoteEventArgs e);

    public class RemoteEventHandler : MarshalByRefObject
    {
        private RemoteEventHandlerCallback callback;
        private SubscriptionInfo subscription;

        public RemoteEventHandler(SubscriptionInfo subscription, RemoteEventHandlerCallback callback)
        {
            this.subscription = subscription;
            this.callback = callback;
        }

        // Helper method to create a generic EventHandler (in the correct domain, which calls the event handler in our domain)
        internal static Delegate CreateProxyHandler(object newSender, EventHandler eventHandler)
        {
            return new EventHandler((_, args) =>
            {
                // We cannot pass the real sender (_) along, since it exists in an unknown domain
                eventHandler?.Invoke(newSender, args);
            });
        }

        public void Invoke(object sender, EventArgs e)
        {
            callback?.Invoke(sender, new RemoteEventArgs(subscription));
        }
    }
}
