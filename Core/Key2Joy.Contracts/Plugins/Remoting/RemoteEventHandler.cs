using System;
using System.Runtime.Serialization;

namespace Key2Joy.Contracts.Plugins.Remoting
{
    [Serializable]
    public class RemoteEventArgs : ISafeSerializationData
    {
        public SubscriptionInfo Subscription { get; set; }

        public RemoteEventArgs(SubscriptionInfo subscription)
        {
            this.Subscription = subscription;
        }

        public void CompleteDeserialization(object deserialized)
        { }
    }

    public delegate void RemoteEventHandlerCallback(object sender, RemoteEventArgs e);

    public class RemoteEventHandler : MarshalByRefObject
    {
        private readonly RemoteEventHandlerCallback callback;
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
            this.callback?.Invoke(sender, new RemoteEventArgs(this.subscription));
        }
    }
}
