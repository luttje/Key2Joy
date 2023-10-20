using Key2Joy.LowLevelInput.XInput;
using Key2Joy.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Key2Joy.Tests.Core.LowLevelInput.XInput;

[TestClass]
public class GamepadTests
{
    [TestMethod]
    public void IsLeftThumbMoved_WithDefaultDeadZoneAndNoMove_ReturnsFalse()
    {
        var gamepad = new XInputGamePad() { LeftThumbX = 0, LeftThumbY = 0 };
        Assert.IsFalse(gamepad.IsLeftThumbMoved());
    }

    [TestMethod]
    public void IsLeftThumbMoved_WithMovePastDefaultDeadZone_ReturnsTrue()
    {
        var gamepad = new XInputGamePad() { LeftThumbX = XInputGamePad.XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE + 1, LeftThumbY = 0 };
        Assert.IsTrue(gamepad.IsLeftThumbMoved());
    }

    [TestMethod]
    public void IsLeftThumbMoved_WithDeltaMarginAndMovePastDelta_ReturnsTrue()
    {
        var gamepad = new XInputGamePad() { LeftThumbX = (int)(0.5f * 32767) + 1, LeftThumbY = 0 };
        Assert.IsTrue(gamepad.IsLeftThumbMoved(new ExactAxisDirection { X = 0.5f, Y = 0 }));
    }

    [TestMethod]
    public void IsLeftThumbMoved_WithDeltaMarginAndNoMovePastDelta_ReturnsFalse()
    {
        var gamepad = new XInputGamePad() { LeftThumbX = (int)(0.4f * 32767), LeftThumbY = 0 };
        Assert.IsFalse(gamepad.IsLeftThumbMoved(new ExactAxisDirection { X = 0.5f, Y = 0 }));
    }

    [TestMethod]
    public void IsRightThumbMoved_WithDefaultDeadZoneAndNoMove_ReturnsFalse()
    {
        var gamepad = new XInputGamePad() { RightThumbX = 0, RightThumbY = 0 };
        Assert.IsFalse(gamepad.IsRightThumbMoved());
    }

    [TestMethod]
    public void IsRightThumbMoved_WithMovePastDefaultDeadZone_ReturnsTrue()
    {
        var gamepad = new XInputGamePad() { RightThumbX = XInputGamePad.XINPUT_GAMEPAD_RIGHT_THUMB_DEADZONE + 1, RightThumbY = 0 };
        Assert.IsTrue(gamepad.IsRightThumbMoved());
    }

    [TestMethod]
    public void IsRightThumbMoved_WithDeltaMarginAndMovePastDelta_ReturnsTrue()
    {
        var gamepad = new XInputGamePad() { RightThumbX = (int)(0.5f * 32767) + 1, RightThumbY = 0 };
        Assert.IsTrue(gamepad.IsRightThumbMoved(new ExactAxisDirection { X = 0.5f, Y = 0 }));
    }

    [TestMethod]
    public void IsRightThumbMoved_WithDeltaMarginAndNoMovePastDelta_ReturnsFalse()
    {
        var gamepad = new XInputGamePad() { RightThumbX = (int)(0.4f * 32767), RightThumbY = 0 };
        Assert.IsFalse(gamepad.IsRightThumbMoved(new ExactAxisDirection { X = 0.5f, Y = 0 }));
    }
}
