using Key2Joy.Contracts.Util;
using Key2Joy.LowLevelInput.XInput;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace Key2Joy.Tests.Core.LowLevelInput.XInput;

[TestClass]
public class XInputServiceTests
{
    private Mock<IXInput> mockXInput;
    private XInputService xInputService;

    [TestInitialize]
    public void TestInitialize()
    {
        this.mockXInput = new Mock<IXInput>();
        this.xInputService = new XInputService(this.mockXInput.Object);
    }

    [TestMethod]
    public void GetState_ShouldNotCallXInputGetStateOnUnregisteredDevice()
    {
        var state = this.xInputService.GetState(1);

        this.mockXInput.Verify(x => x.XInputGetState(It.IsAny<int>(), ref It.Ref<XInputState>.IsAny), Times.Never);
        Assert.IsNull(state);
    }

    [TestMethod]
    public void GetCapabilities_ShouldCallXInputGetCapabilities()
    {
        this.xInputService.GetCapabilities(1);

        this.mockXInput.Verify(x => x.XInputGetCapabilities(It.IsAny<int>(), ref It.Ref<XInputCapabilities>.IsAny), Times.Once);
    }

    [TestMethod]
    public void GetBatteryInformation_ShouldCallXInputGetBatteryInformation()
    {
        this.xInputService.GetBatteryInformation(1, BatteryDeviceType.BATTERY_DEVTYPE_GAMEPAD);

        this.mockXInput.Verify(x => x.XInputGetBatteryInformation(It.IsAny<int>(), It.IsAny<BatteryDeviceType>(), ref It.Ref<XInputBatteryInformation>.IsAny), Times.Once);
    }

    [TestMethod]
    public void GetKeystroke_ShouldCallXInputGetKeystroke()
    {
        this.xInputService.GetKeystroke(1);

        this.mockXInput.Verify(x => x.XInputGetKeystroke(It.IsAny<int>(), It.IsAny<int>(), ref It.Ref<XInputKeystroke>.IsAny), Times.Once);
    }

    [TestMethod]
    public void Vibrate_ShouldCallXInputSetState()
    {
        this.xInputService.Vibrate(1, 0.5, 0.5, TimeSpan.FromSeconds(5));

        this.mockXInput.Verify(x => x.XInputSetState(It.IsAny<int>(), ref It.Ref<XInputVibration>.IsAny), Times.Once);
    }

    [TestMethod]
    public void StopVibration_ShouldCallXInputSetStateWithZeroValues()
    {
        this.xInputService.StopVibration(1);

        this.mockXInput.Verify(x => x.XInputSetState(It.IsAny<int>(), ref It.Ref<XInputVibration>.IsAny), Times.Once);

        var vibrationInfo = (XInputVibration)this.mockXInput.Invocations[0].Arguments[1];

        Assert.AreEqual(0, vibrationInfo.LeftMotorSpeed);
        Assert.AreEqual(0, vibrationInfo.RightMotorSpeed);
    }

    [TestMethod]
    public async Task Vibration_Stops_AfterGivenTime()
    {
        var duration = TimeSpan.FromMilliseconds(100);

        // Start vibration
        this.xInputService.Vibrate(1, 0.5, 0.5, duration);

        // Wait for the duration + a little buffer to prevent false positives
        await Task.Delay(duration + TimingHelper.FromMilliseconds(500));

        // Verify that XInputSetState was called to stop vibration after the duration
        this.mockXInput.Verify(x => x.XInputSetState(It.IsAny<int>(), ref It.Ref<XInputVibration>.IsAny), Times.Exactly(2));

        var vibrationInfo = (XInputVibration)this.mockXInput.Invocations[1].Arguments[1];

        Assert.AreEqual(0, vibrationInfo.LeftMotorSpeed);
        Assert.AreEqual(0, vibrationInfo.RightMotorSpeed);
    }
}
