using System.Diagnostics;
using Key2Joy.Mapping.Actions.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Key2Joy.Tests.Core.Mapping.Action.Windows
{
    [TestClass]
    public class WindowFindActionTest
    {
        /// <summary>
        /// Tests if a created process' main window can be found by it's title.
        /// </summary>
        /// <param name="processName"></param>
        [TestMethod]
        [DataRow("notepad.exe")]
        [DataRow("cmd.exe")]
        public void Script_Finds_CreatedProcess(string processName)
        {
            WindowFindAction action = new(string.Empty);
            var process = Process.Start(new ProcessStartInfo
            {
                FileName = processName,
            });

            var actualHandle = WindowHelper.ResolveMainWindowHandle(process);
            var handle = action.ExecuteForScript(null, process.MainWindowTitle);
            process.Kill();

            Assert.AreEqual(actualHandle, handle);
        }
    }
}
