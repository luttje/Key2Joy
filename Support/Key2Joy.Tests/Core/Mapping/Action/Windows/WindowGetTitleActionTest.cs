using System.Diagnostics;
using Key2Joy.Mapping.Actions.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Key2Joy.Tests.Core.Mapping.Action.Windows;

[TestClass]
public class WindowGetTitleActionTest
{
    [TestInitialize]
    public void Setup()
    {
        var lang = System.Globalization.CultureInfo.InstalledUICulture;
        Assert.AreEqual(lang.Name, "en-US", "Theses tests must be run with en-US culture.");
    }

    /// <summary>
    /// Tests if the Window Get Title Action can return the proper title.
    /// </summary>
    /// <param name="processName"></param>
    /// <param name="expectedTitle"></param>
    [TestMethod]
    [DataRow("notepad.exe", "Untitled - Notepad")]
    [DataRow("cmd.exe", @"C:\Windows\System32\cmd.exe")]
    public void Script_Returns_CreatedWindowTitle(string processName, string expectedTitle)
    {
        WindowGetTitleAction action = new(string.Empty);
        var process = Process.Start(new ProcessStartInfo
        {
            FileName = processName,
        });

        // Wait for the handle to become active
        WindowHelper.ResolveMainWindowHandle(process);

        var result = action.ExecuteForScript(process.MainWindowHandle);
        process.Kill();

        Assert.AreEqual(expectedTitle, result.Replace("Administrator: ", string.Empty)); // On CI, the process is run as Administrator
    }
}
