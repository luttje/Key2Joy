using System.Windows.Forms;
using Key2Joy.LowLevelInput;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Key2Joy.Tests.Core.LowLevelInput;

[TestClass]
public class VirtualKeyConverterTests
{
    private const short Shift = 0x0100;
    private const short Ctrl = 0x0200;
    private const short Alt = 0x0400;

    private Mock<IVirtualKeyService> mockService;
    private VirtualKeyConverter converter;

    [TestInitialize]
    public void Setup()
    {
        this.mockService = new Mock<IVirtualKeyService>();
        this.converter = new VirtualKeyConverter(this.mockService.Object);
    }

    [TestMethod]
    public void TestKeysFromVirtual()
    {
        var result = this.converter.KeysFromVirtual(65); // ASCII for 'A'
        Assert.AreEqual(Keys.A, result);
    }

    [TestMethod]
    public void TestKeysFromScanCode()
    {
        // Mock the service response
        this.mockService.Setup(m => m.MapVirtualKey(It.IsAny<uint>(), It.IsAny<MapVirtualKeyMapTypes>())).Returns(65u);

        var result = this.converter.KeysFromScanCode(KeyboardKey.Key_A);
        Assert.AreEqual(Keys.A, result);
    }

    [TestMethod]
    public void KeysFromChar_WithShiftKey_ReturnsShiftAndCharKey()
    {
        var testChar = 'A'; // An example character
        short mockValue = 0x41 | Shift; // 'A' with SHIFT
        this.mockService.Setup(m => m.VkKeyScan(testChar)).Returns(mockValue);

        var result = this.converter.KeysFromChar(testChar);

        Assert.IsTrue(result.HasFlag(Keys.Shift) && result.HasFlag(Keys.A));
    }

    [TestMethod]
    public void KeysFromChar_WithControlKey_ReturnsControlAndCharKey()
    {
        var testChar = 'B'; // An example character
        short mockValue = 0x42 | Ctrl; // 'B' with CTRL
        this.mockService.Setup(m => m.VkKeyScan(testChar)).Returns(mockValue);

        var result = this.converter.KeysFromChar(testChar);

        Assert.IsTrue(result.HasFlag(Keys.Control) && result.HasFlag(Keys.B));
    }

    [TestMethod]
    public void KeysFromChar_WithAltKey_ReturnsAltAndCharKey()
    {
        var testChar = 'C'; // An example character
        short mockValue = 0x43 | Alt; // 'C' with ALT
        this.mockService.Setup(m => m.VkKeyScan(testChar)).Returns(mockValue);

        var result = this.converter.KeysFromChar(testChar);

        Assert.IsTrue(result.HasFlag(Keys.Alt) && result.HasFlag(Keys.C));
    }

    [TestMethod]
    public void KeysToString_ValidKey_ReturnsExpectedString()
    {
        var result = this.converter.KeysToString(Keys.A);

        Assert.AreEqual("A", result);
    }
}
