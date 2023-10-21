using System;
using System.Linq;
using CommonServiceLocator;
using Key2Joy.LowLevelInput.SimulatedGamePad;
using Key2Joy.LowLevelInput.XInput;
using Key2Joy.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Key2Joy.Tests.Core.LowLevelInput.SimulatedGamePad;

[TestClass]
public class SimulatedGamePadTests
{
    private const int NUM_GAMEPADS = 4;
    private SimulatedGamePadService gamePadService;
    private Mock<ISimulatedGamePad>[] mockedGamePads;

    [TestInitialize]
    public void Initialize()
    {
        var serviceLocator = new DependencyServiceLocator();
        ServiceLocator.SetLocatorProvider(() => serviceLocator);
        serviceLocator.Register<IXInputService>(new XInputService());

        this.mockedGamePads = new Mock<ISimulatedGamePad>[NUM_GAMEPADS];
        for (var i = 0; i < NUM_GAMEPADS; i++)
        {
            var isPluggedIn = false;
            this.mockedGamePads[i] = new Mock<ISimulatedGamePad>();
            this.mockedGamePads[i].Setup(g => g.GetIsPluggedIn()).Returns(() => isPluggedIn);
            this.mockedGamePads[i].Setup(g => g.PlugIn()).Callback(() => isPluggedIn = true);
            this.mockedGamePads[i].Setup(g => g.Unplug()).Callback(() => isPluggedIn = false);
            this.mockedGamePads[i].Setup(g => g.Index).Returns(i);
        }

        this.gamePadService = new SimulatedGamePadService(this.mockedGamePads.Select(m => m.Object).ToArray());

        // this.gamePadService.Initialize(); // Not needed since we mock gamepads
        // This saves us installing the virtual bus and drivers (possibly failing in CI, I don't even want to try)
        // in any case I don't feel like we need to test a third-party library which has already proven itself
        //
        // Not needed for the same reasons:
        // [TestCleanup]
        // public void Cleanup() => this.gamePadService.ShutDown();
    }

    [TestMethod]
    public void SimulatedGamePadService_ConstructsAllGamePads()
    {
        var gamePads = this.gamePadService.GetAllGamePads(false);

        Assert.AreEqual(NUM_GAMEPADS, gamePads.Length);
        for (var i = 0; i < NUM_GAMEPADS; i++)
        {
            Assert.IsNotNull(gamePads[i]);
            Assert.AreEqual(i, gamePads[i].Index);
        }
    }

    [TestMethod]
    public void SimulatedGamePadService_EnsurePluggedIn_PlugsInUnpluggedGamePad()
    {
        const int gamePadIndexToTest = 2;
        var gamePad = this.gamePadService.GetGamePad(gamePadIndexToTest);

        this.gamePadService.EnsurePluggedIn(gamePadIndexToTest);
        Assert.IsTrue(gamePad.GetIsPluggedIn());
    }

    [TestMethod]
    public void SimulatedGamePadService_EnsurePluggedIn_ThrowsForInvalidIndex() => Assert.ThrowsException<ArgumentOutOfRangeException>(() => this.gamePadService.EnsurePluggedIn(5));

    [TestMethod]
    public void SimulatedGamePadService_EnsureAllUnplugged_UnplugsAll()
    {
        foreach (var gamePad in this.gamePadService.GetAllGamePads())
        {
            gamePad.PlugIn();
        }

        this.gamePadService.EnsureAllUnplugged();
        Assert.IsFalse(this.gamePadService.GetAllGamePads().Any(gp => gp.GetIsPluggedIn()));
    }
}
