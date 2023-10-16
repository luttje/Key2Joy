using System;
using CommonServiceLocator;
using Key2Joy.LowLevelInput.GamePad;
using Key2Joy.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Key2Joy.Tests.Core.Util;

public interface IAnotherService
{ }

public class TestService : IAnotherService
{ }

public class TestAnotherGamePadService : IGamePadService
{
    public void Initialize()
    { }

    public void ShutDown()
    { }

    public IGamePad GetGamePad(int gamePadIndex) => null;

    public IGamePad[] GetAllGamePads() => Array.Empty<IGamePad>();

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
        serviceLocator.Register<IGamePadService>(new SimulatedGamePadService());

        var service = serviceLocator.GetInstance<IGamePadService>();
        Assert.IsNotNull(service);
        Assert.IsInstanceOfType(service, typeof(SimulatedGamePadService));
    }

    [TestMethod]
    [ExpectedException(typeof(ActivationException))]
    public void Retrieve_Unregistered_Service_Throws_Exception()
    {
        var serviceLocator = new DependencyServiceLocator();
        serviceLocator.GetInstance<IGamePadService>();
    }

    [TestMethod]
    public void Can_Register_Multiple_Services()
    {
        var serviceLocator = new DependencyServiceLocator();
        serviceLocator.Register<IGamePadService>(new SimulatedGamePadService());
        serviceLocator.Register<IAnotherService>(new TestService());

        Assert.IsNotNull(serviceLocator.GetInstance<IGamePadService>());
        Assert.IsNotNull(serviceLocator.GetInstance<IAnotherService>());
    }

    [TestMethod]
    public void Registered_Service_Is_Singleton_By_Default()
    {
        var serviceLocator = new DependencyServiceLocator();
        serviceLocator.Register<IGamePadService>(new SimulatedGamePadService());

        var service1 = serviceLocator.GetInstance<IGamePadService>();
        var service2 = serviceLocator.GetInstance<IGamePadService>();

        Assert.AreSame(service1, service2);
    }

    [TestMethod]
    public void Can_Override_Registered_Service()
    {
        var serviceLocator = new DependencyServiceLocator();
        serviceLocator.Register<IGamePadService>(new SimulatedGamePadService());
        serviceLocator.Register<IGamePadService>(new TestAnotherGamePadService());

        var service = serviceLocator.GetInstance<IGamePadService>();

        Assert.IsInstanceOfType(service, typeof(TestAnotherGamePadService));
    }

    [TestMethod]
    public void Can_Use_Generic_GetInstance()
    {
        var serviceLocator = new DependencyServiceLocator();
        serviceLocator.Register<IGamePadService>(new SimulatedGamePadService());

        var service = serviceLocator.GetInstance<IGamePadService>();

        Assert.IsInstanceOfType(service, typeof(SimulatedGamePadService));
    }

    [TestMethod]
    public void Can_Use_NonGeneric_GetInstance()
    {
        var serviceLocator = new DependencyServiceLocator();
        serviceLocator.Register<IGamePadService>(new SimulatedGamePadService());

        var service = serviceLocator.GetInstance(typeof(IGamePadService));

        Assert.IsInstanceOfType(service, typeof(SimulatedGamePadService));
    }

    [TestMethod]
    [ExpectedException(typeof(ActivationException))]
    public void NonGeneric_GetInstance_Unregistered_Service_Throws_Exception()
    {
        var serviceLocator = new DependencyServiceLocator();
        serviceLocator.GetInstance(typeof(IGamePadService));
    }

    [TestMethod]
    public void ServiceLocator_SetLocatorProvider_Works()
    {
        var serviceLocator = new DependencyServiceLocator();
        ServiceLocator.SetLocatorProvider(() => serviceLocator);

        serviceLocator.Register<IGamePadService>(new SimulatedGamePadService());

        var service = ServiceLocator.Current.GetInstance<IGamePadService>();

        Assert.IsNotNull(service);
    }
}
