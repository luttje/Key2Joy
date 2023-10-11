namespace Key2Joy.Contracts.Plugins.Remoting;

public class SubscriptionRegistration
{
    public SubscriptionTicket Ticket { get; private set; }
    public RemoteEventHandlerCallback EventHandler { get; private set; }
    public object CustomSender { get; set; }

    public SubscriptionRegistration(string eventName, string subscriptionId, RemoteEventHandlerCallback eventHandler, object customSender = null)
    {
        this.Ticket = new SubscriptionTicket(eventName, subscriptionId);
        this.EventHandler = eventHandler;
        this.CustomSender = customSender;
    }
}
