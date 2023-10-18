using System;

namespace Key2Joy.LowLevelInput;

public interface IHookManager
{
    IntPtr LoadLibrary(string lpFileName);

    bool FreeLibrary(IntPtr hModule);

    IntPtr SetWindowsHookEx(int idHook, GlobalInputHook.HookProc lpfn, IntPtr hMod, int dwThreadId);

    bool UnhookWindowsHookEx(IntPtr hHook);

    IntPtr CallNextHookEx(IntPtr hHook, int code, IntPtr wParam, IntPtr lParam);
}
