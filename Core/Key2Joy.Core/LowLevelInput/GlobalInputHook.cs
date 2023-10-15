using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Key2Joy.LowLevelInput;

// Based on these sources:
// - https://gist.github.com/Stasonix/3181083
// - https://stackoverflow.com/a/34384189
public class GlobalInputHook : IDisposable
{
    public const int WH_KEYBOARD_LL = 13;
    public const int WH_MOUSE_LL = 14;

    public event EventHandler<GlobalKeyboardHookEventArgs> KeyboardInputEvent;

    public event EventHandler<GlobalMouseHookEventArgs> MouseInputEvent;

    public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

    private readonly IntPtr[] windowsHookHandles;
    private IntPtr user32LibraryHandle;
    private HookProc hookProc;
    private readonly IHookManager hookManager;

    public GlobalInputHook(IHookManager hookManager = null)
    {
        this.hookManager = hookManager ?? new NativeHookManager();
        this.windowsHookHandles = new IntPtr[2];
        this.user32LibraryHandle = IntPtr.Zero;

        this.Initialize();
    }

    private void Initialize()
    {
        this.hookProc = this.LowLevelInputHook; // keep alive hookProc

        this.user32LibraryHandle = this.hookManager.LoadLibrary("User32");
        if (this.user32LibraryHandle == IntPtr.Zero)
        {
            var errorCode = Marshal.GetLastWin32Error();
            throw new Win32Exception(errorCode, $"Failed to load library 'User32.dll'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
        }

        var windowsHooks = new int[] { WH_KEYBOARD_LL, WH_MOUSE_LL };

        for (var i = 0; i < windowsHooks.Length; i++)
        {
            var windowsHook = windowsHooks[i];

            this.windowsHookHandles[i] = this.hookManager.SetWindowsHookEx(windowsHook, this.hookProc, this.user32LibraryHandle, 0);

            if (this.windowsHookHandles[i] == IntPtr.Zero)
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
            // Because we can unhook only in the same thread, not in garbage collector thread
            for (var i = 0; i < this.windowsHookHandles.Length; i++)
            {
                var windowsHookHandle = this.windowsHookHandles[i];

                if (windowsHookHandle != IntPtr.Zero)
                {
                    if (!this.hookManager.UnhookWindowsHookEx(windowsHookHandle))
                    {
                        var errorCode = Marshal.GetLastWin32Error();
                        throw new Win32Exception(errorCode, $"Failed to remove input hooks for '{Process.GetCurrentProcess().ProcessName}'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
                    }
                    this.windowsHookHandles[i] = IntPtr.Zero;

                    // ReSharper disable once DelegateSubtraction
                    this.hookProc -= this.LowLevelInputHook;
                }
            }
        }

        if (this.user32LibraryHandle != IntPtr.Zero)
        {
            if (!this.hookManager.FreeLibrary(this.user32LibraryHandle)) // reduces reference to library by 1.
            {
                var errorCode = Marshal.GetLastWin32Error();
                throw new Win32Exception(errorCode, $"Failed to unload library 'User32.dll'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
            }
            this.user32LibraryHandle = IntPtr.Zero;
        }
    }

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

        return isInputHandled ? (IntPtr)1 : this.hookManager.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
    }

    ~GlobalInputHook()
    {
        this.Dispose(false);
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }
}
