using System;
using System.Diagnostics;

namespace Key2Joy.Tests.Core.Mapping.Action.Windows;

internal class WindowHelper
{
    /// <summary>
    /// Waits for the window handle to become active (non-zero pointer) and returns it.
    /// </summary>
    /// <param name="process"></param>
    /// <returns>Non Null-Pointer Handle to the Process Main Window Handle</returns>
    internal static IntPtr ResolveMainWindowHandle(Process process)
    {
        var handle = IntPtr.Zero;

        while (!process.HasExited)
        {
            process.Refresh();
            if (IntPtr.Zero != process.MainWindowHandle)
            {
                handle = process.MainWindowHandle;
                break;
            }
        }

        return handle;
    }
}
