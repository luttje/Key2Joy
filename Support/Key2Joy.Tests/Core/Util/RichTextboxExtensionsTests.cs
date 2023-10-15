using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Windows.Forms;
using Key2Joy.Util;

namespace Key2Joy.Tests.Core.Util;

[TestClass]
public class RichTextboxExtensionsTests
{
    [TestMethod]
    public void AppendText_AppendsTextWithGivenColor()
    {
        var mockRichTextBox = new Mock<RichTextBox>() { CallBase = true };
        var appendColor = Color.Red;

        mockRichTextBox.Object.AppendText("Test ", appendColor);

        Assert.AreEqual("Test ", mockRichTextBox.Object.Text);
    }
}
