using Key2Joy.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Threading;

namespace Key2Joy.Tests.Core.Mapping.Action.Windows
{
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
        /// TODO: This test is super unstable because:
        /// - The window may take longer to load
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="expectedTitle"></param>
        [TestMethod]
        [DataRow("notepad.exe", "Untitled - Notepad")]
        [DataRow("cmd.exe", @"C:\Windows\System32\cmd.exe")]
        public void Script_Returns_CreatedWindowTitle(string processName, string expectedTitle)
        {
            var action = new WindowGetTitleAction(string.Empty, string.Empty);
            var process = Process.Start(new ProcessStartInfo
            {
                FileName = processName,
            });

            // Wait for the handle to become active
            WindowHelper.ResolveMainWindowHandle(process);

            var result = action.ExecuteForScript(process.MainWindowHandle);
            process.Kill();

            Assert.AreEqual(expectedTitle, result);
        }
    }
}
