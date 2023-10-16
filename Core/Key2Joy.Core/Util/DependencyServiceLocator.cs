using System;
using System.Collections.Generic;
using CommonServiceLocator;

namespace Key2Joy.Util;

public class DependencyServiceLocator : ServiceLocatorImplBase
{
    private readonly Dictionary<Type, object> services = new();

    protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        => throw new NotImplementedException();

    protected override object DoGetInstance(Type serviceType, string key)
    {
        if (this.services.TryGetValue(serviceType, out var service))
            return service;

        throw new InvalidOperationException($"No registered service of type {serviceType.FullName}");
    }

    public void Register<T>(T service) => this.services[typeof(T)] = service;
}
