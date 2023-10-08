using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Key2Joy.LowLevelInput
{
    // Based on these sources:
    // - https://gist.github.com/Stasonix/3181083
    // - https://stackoverflow.com/a/34384189
    public partial class GlobalInputHook : IDisposable
    {
        public event EventHandler<GlobalKeyboardHookEventArgs> KeyboardInputEvent;
        public event EventHandler<GlobalMouseHookEventArgs> MouseInputEvent;

        private readonly IntPtr[] windowsHookHandles;
        private IntPtr user32LibraryHandle;
        private HookProc hookProc;

        private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool FreeLibrary(IntPtr hModule);

        /// <summary>
        /// The SetWindowsHookEx function installs an application-defined hook procedure into a hook chain.
        /// You would install a hook procedure to monitor the system for certain types of events. These events are
        /// associated either with a specific thread or with all threads in the same desktop as the calling thread.
        /// </summary>
        /// <param name="idHook">hook type</param>
        /// <param name="lpfn">hook procedure</param>
        /// <param name="hMod">handle to application instance</param>
        /// <param name="dwThreadId">thread identifier</param>
        /// <returns>If the function succeeds, the return value is the handle to the hook procedure.</returns>
        [DllImport("USER32", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, int dwThreadId);

        /// <summary>
        /// The UnhookWindowsHookEx function removes a hook procedure installed in a hook chain by the SetWindowsHookEx function.
        /// </summary>
        /// <param name="hhk">handle to hook procedure</param>
        /// <returns>If the function succeeds, the return value is true.</returns>
        [DllImport("USER32", SetLastError = true)]
        public static extern bool UnhookWindowsHookEx(IntPtr hHook);

        /// <summary>
        /// The CallNextHookEx function passes the hook information to the next hook procedure in the current hook chain.
        /// A hook procedure can call this function either before or after processing the hook information.
        /// </summary>
        /// <param name="hHook">handle to current hook</param>
        /// <param name="code">hook code passed to hook procedure</param>
        /// <param name="wParam">value passed to hook procedure</param>
        /// <param name="lParam">value passed to hook procedure</param>
        /// <returns>If the function succeeds, the return value is true.</returns>
        [DllImport("USER32", SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hHook, int code, IntPtr wParam, IntPtr lParam);

        public GlobalInputHook()
        {
            windowsHookHandles = new IntPtr[2];
            user32LibraryHandle = IntPtr.Zero;
            hookProc = LowLevelInputHook; // we must keep alive hookProc, because GC is not aware about SetWindowsHookEx behaviour.

            user32LibraryHandle = LoadLibrary("User32");
            if (user32LibraryHandle == IntPtr.Zero)
            {
                var errorCode = Marshal.GetLastWin32Error();
                throw new Win32Exception(errorCode, $"Failed to load library 'User32.dll'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
            }

            var windowsHooks = new int[] { WH_KEYBOARD_LL, WH_MOUSE_LL };

            for (var i = 0; i < windowsHooks.Length; i++)
            {
                var windowsHook = windowsHooks[i];

                windowsHookHandles[i] = SetWindowsHookEx(windowsHook, hookProc, user32LibraryHandle, 0);

                if (windowsHookHandles[i] == IntPtr.Zero)
                {
                    var errorCode = Marshal.GetLastWin32Error();
                    throw new Win32Exception(errorCode, $"Failed to adjust input hooks for '{Process.GetCurrentProcess().ProcessName}'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // because we can unhook only in the same thread, not in garbage collector thread
                for (var i = 0; i < windowsHookHandles.Length; i++)
                {
                    var windowsHookHandle = windowsHookHandles[i];

                    if (windowsHookHandle != IntPtr.Zero)
                    {
                        if (!UnhookWindowsHookEx(windowsHookHandle))
                        {
                            var errorCode = Marshal.GetLastWin32Error();
                            throw new Win32Exception(errorCode, $"Failed to remove input hooks for '{Process.GetCurrentProcess().ProcessName}'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
                        }
                        windowsHookHandles[i] = IntPtr.Zero;

                        // ReSharper disable once DelegateSubtraction
                        hookProc -= LowLevelInputHook;
                    }
                }
            }

            if (user32LibraryHandle != IntPtr.Zero)
            {
                if (!FreeLibrary(user32LibraryHandle)) // reduces reference to library by 1.
                {
                    var errorCode = Marshal.GetLastWin32Error();
                    throw new Win32Exception(errorCode, $"Failed to unload library 'User32.dll'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
                }
                user32LibraryHandle = IntPtr.Zero;
            }
        }

        ~GlobalInputHook()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public const int WH_KEYBOARD_LL = 13;
        public const int WH_MOUSE_LL = 14;

        public IntPtr LowLevelInputHook(int nCode, IntPtr wParam, IntPtr lParam)
        {
            var isInputHandled = false;
            var wparamTyped = wParam.ToInt32();

            if (Enum.IsDefined(typeof(KeyboardState), wparamTyped))
            {
                var o = Marshal.PtrToStructure(lParam, typeof(LowLevelKeyboardInputEvent));
                var p = (LowLevelKeyboardInputEvent)o;

                GlobalKeyboardHookEventArgs eventArguments = new(p, (KeyboardState)wparamTyped);

                var handler = KeyboardInputEvent;
                handler?.Invoke(this, eventArguments);

                isInputHandled = eventArguments.Handled;
            }
            else if (Enum.IsDefined(typeof(MouseState), wparamTyped))
            {
                var o = Marshal.PtrToStructure(lParam, typeof(LowLevelMouseInputEvent));
                var p = (LowLevelMouseInputEvent)o;

                GlobalMouseHookEventArgs eventArguments = new(p, (MouseState)wparamTyped);

                var handler = MouseInputEvent;
                handler?.Invoke(this, eventArguments);

                isInputHandled = eventArguments.Handled;
            }

            return isInputHandled ? (IntPtr)1 : CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }
    }
}
