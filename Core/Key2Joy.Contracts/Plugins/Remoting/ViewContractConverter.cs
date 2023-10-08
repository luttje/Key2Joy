using System;
using System.AddIn.Contract;
using System.AddIn.Pipeline;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Windows;

namespace Key2Joy.Contracts.Plugins.Remoting;

public class ViewContractConverter : MarshalByRefObject
{
    private object instance;

    public INativeHandleContract ConvertToContract(FrameworkElement element)
    {
        var contract = FrameworkElementAdapters.ViewToContractAdapter(element);
        return contract;
    }

    public INativeHandleContract ConvertToContract(ObjectHandle controlHandle, Dictionary<string, RemoteEventHandler> boundEvents)
    {
        this.instance = controlHandle.Unwrap();

        // Iterate the events in this type and bind to them. Call AnyEvent when one happens
        var type = this.instance.GetType();
        var events = type.GetEvents();

        foreach (var e in events)
        {
            // Check if the event name is in the boundEvents dictionary
            if (boundEvents.ContainsKey(e.Name))
            {
                //if (IsValidDelegateType(e.EventHandlerType))
                //{
                var handler = RemoteEventHandler.CreateProxyHandler(controlHandle, boundEvents[e.Name].Invoke);
                e.AddEventHandler(this.instance, handler);
                Console.WriteLine($"Binding to event: {e.Name}");
                //}
                //else
                //{
                //    Console.WriteLine($"Cannot add event handler to {e.Name}. Invalid delegate type. Should be {e.EventHandlerType}");
                //}
            }
        }

        // Check for events specified in boundEvents but not found in the type
        var unknownEvents = boundEvents.Keys.Except(events.Select(eventInfo => eventInfo.Name));
        foreach (var unknownEvent in unknownEvents)
        {
            Console.WriteLine($"Event {unknownEvent} is specified in bound events but does not exist in the actual type {type.FullName}.");
        }

        return this.ConvertToContract((FrameworkElement)this.instance);
    }

    // Helper method to check if the delegate type is compatible with EventHandler
    //bool IsValidDelegateType(Type delegateType)
    //{
    //    if (delegateType == null)
    //        return false;

    //    var eventHandlerType = typeof(EventHandler);
    //    var method = delegateType.GetMethod("Invoke");

    //    if (method == null)
    //        return false;

    //    // Check if the delegate type has the same method signature as EventHandler
    //    return method.ReturnType == typeof(void) &&
    //           method.GetParameters()
    //               .SequenceEqual(eventHandlerType.GetMethod("Invoke")?.GetParameters());
    //}

    public object RemoteInvoke(string methodName, object[] parameters)
    {
        var type = this.instance.GetType();
        var mi = type.GetMethod(methodName);
        return mi.Invoke(this.instance, parameters);
    }
}
