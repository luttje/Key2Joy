using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Key2Joy.Mapping.Actions.Graphics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Key2Joy.Tests.Core.Mapping.Actions.Graphics;

[TestClass]
public class CaptureScreenActionTests
{
    private const string TEST_IMAGE_PATH = @"ImageTests";
    private CaptureScreenAction action;

    [TestInitialize]
    public void Initialize()
    {
        this.action = new CaptureScreenAction("Capture Screen");

        Directory.CreateDirectory(TEST_IMAGE_PATH);
    }

    [TestCleanup]
    public void Cleanup()
        => Directory.Delete(TEST_IMAGE_PATH, true);

    [TestMethod]
    public void ExposesScriptingMethod_DefaultValuesAreSetWhenNotProvided()
    {
        var savePath = $"{TEST_IMAGE_PATH}/tempPath.jpg";

        this.action.ExecuteForScript(savePath);

        using var image = Image.FromFile(savePath);

        var expectedWidth = SystemInformation.VirtualScreen.Width;
        var expectedHeight = SystemInformation.VirtualScreen.Height;

        Assert.AreEqual(expectedWidth, image.Width);
        Assert.AreEqual(expectedHeight, image.Height);
    }

    [TestMethod]
    public void ExposesScriptingMethod_CanSaveSpecificRegion()
    {
        var savePath = $"{TEST_IMAGE_PATH}/tempPath.jpg";

        this.action.ExecuteForScript(savePath, 0, 0, 100, 100);

        using var image = Image.FromFile(savePath);

        Assert.AreEqual(100, image.Width);
        Assert.AreEqual(100, image.Height);
    }
}
