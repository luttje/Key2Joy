using System;
using CommonServiceLocator;
using Key2Joy.LowLevelInput.SimulatedGamePad;
using Key2Joy.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Key2Joy.Tests.Core.Util;

public interface IAnotherService
{ }

public class TestService : IAnotherService
{ }

public class TestAnotherGamePadService : ISimulatedGamePadService
{
    public void Initialize()
    { }

    public void ShutDown()
    { }

    public ISimulatedGamePad GetGamePad(int gamePadIndex) => null;

    public ISimulatedGamePad[] GetAllGamePads(bool onlyPluggedIn = true) => Array.Empty<ISimulatedGamePad>();

    public void EnsureAllUnplugged()
    { }

    public void EnsurePluggedIn(int gamePadIndex)
    { }

    public void EnsureUnplugged(int gamePadIndex)
    { }
}

[TestClass]
public class DependencyServiceLocatorTests
{
    [TestMethod]
    public void Register_And_Retrieve_Service()
    {
        var serviceLocator = new DependencyServiceLocator();
        serviceLocator.Register<ISimulatedGamePadService>(new SimulatedGamePadService());

        var service = serviceLocator.GetInstance<ISimulatedGamePadService>();
        Assert.IsNotNull(service);
        Assert.IsInstanceOfType(service, typeof(SimulatedGamePadService));
    }

    [TestMethod]
    [ExpectedException(typeof(ActivationException))]
    public void Retrieve_Unregistered_Service_Throws_Exception()
    {
        var serviceLocator = new DependencyServiceLocator();
        serviceLocator.GetInstance<ISimulatedGamePadService>();
    }

    [TestMethod]
    public void Can_Register_Multiple_Services()
    {
        var serviceLocator = new DependencyServiceLocator();
        serviceLocator.Register<ISimulatedGamePadService>(new SimulatedGamePadService());
        serviceLocator.Register<IAnotherService>(new TestService());

        Assert.IsNotNull(serviceLocator.GetInstance<ISimulatedGamePadService>());
        Assert.IsNotNull(serviceLocator.GetInstance<IAnotherService>());
    }

    [TestMethod]
    public void Registered_Service_Is_Singleton_By_Default()
    {
        var serviceLocator = new DependencyServiceLocator();
        serviceLocator.Register<ISimulatedGamePadService>(new SimulatedGamePadService());

        var service1 = serviceLocator.GetInstance<ISimulatedGamePadService>();
        var service2 = serviceLocator.GetInstance<ISimulatedGamePadService>();

        Assert.AreSame(service1, service2);
    }

    [TestMethod]
    public void Can_Override_Registered_Service()
    {
        var serviceLocator = new DependencyServiceLocator();
        serviceLocator.Register<ISimulatedGamePadService>(new SimulatedGamePadService());
        serviceLocator.Register<ISimulatedGamePadService>(new TestAnotherGamePadService());

        var service = serviceLocator.GetInstance<ISimulatedGamePadService>();

        Assert.IsInstanceOfType(service, typeof(TestAnotherGamePadService));
    }

    [TestMethod]
    public void Can_Use_Generic_GetInstance()
    {
        var serviceLocator = new DependencyServiceLocator();
        serviceLocator.Register<ISimulatedGamePadService>(new SimulatedGamePadService());

        var service = serviceLocator.GetInstance<ISimulatedGamePadService>();

        Assert.IsInstanceOfType(service, typeof(SimulatedGamePadService));
    }

    [TestMethod]
    public void Can_Use_NonGeneric_GetInstance()
    {
        var serviceLocator = new DependencyServiceLocator();
        serviceLocator.Register<ISimulatedGamePadService>(new SimulatedGamePadService());

        var service = serviceLocator.GetInstance(typeof(ISimulatedGamePadService));

        Assert.IsInstanceOfType(service, typeof(SimulatedGamePadService));
    }

    [TestMethod]
    [ExpectedException(typeof(ActivationException))]
    public void NonGeneric_GetInstance_Unregistered_Service_Throws_Exception()
    {
        var serviceLocator = new DependencyServiceLocator();
        serviceLocator.GetInstance(typeof(ISimulatedGamePadService));
    }

    [TestMethod]
    public void ServiceLocator_SetLocatorProvider_Works()
    {
        var serviceLocator = new DependencyServiceLocator();
        ServiceLocator.SetLocatorProvider(() => serviceLocator);

        serviceLocator.Register<ISimulatedGamePadService>(new SimulatedGamePadService());

        var service = ServiceLocator.Current.GetInstance<ISimulatedGamePadService>();

        Assert.IsNotNull(service);
    }
}
