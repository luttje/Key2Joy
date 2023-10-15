using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using static Key2Joy.LowLevelInput.GlobalInputHook;

namespace Key2Joy.LowLevelInput;

/// <summary>
/// Represents a class for managing native hooks.
/// </summary>
[ExcludeFromCodeCoverage]
public class NativeHookManager : IHookManager
{
    /// <summary>
    /// Loads a dynamic-link library (DLL) into the address space of the calling process.
    /// </summary>
    /// <param name="lpFileName">The name of the DLL to load.</param>
    /// <returns>A handle to the loaded DLL if successful; otherwise, IntPtr.Zero.</returns>
    [DllImport("kernel32.dll", EntryPoint = "LoadLibrary")]
    private static extern IntPtr InternalLoadLibrary(string lpFileName);

    /// <summary>
    /// Frees the loaded dynamic-link library (DLL).
    /// </summary>
    /// <param name="hModule">A handle to the loaded DLL.</param>
    /// <returns>true if successful, false otherwise.</returns>
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, EntryPoint = "FreeLibrary")]
    private static extern bool InternalFreeLibrary(IntPtr hModule);

    /// <summary>
    /// Installs an application-defined hook procedure into a hook chain.
    /// </summary>
    /// <param name="idHook">The type of hook to be installed.</param>
    /// <param name="lpfn">A pointer to the hook procedure.</param>
    /// <param name="hMod">A handle to the DLL containing the hook procedure.</param>
    /// <param name="dwThreadId">The identifier of the thread with which the hook procedure is to be associated.</param>
    /// <returns>A handle to the hook procedure if successful; otherwise, IntPtr.Zero.</returns>
    [DllImport("user32.dll", SetLastError = true, EntryPoint = "SetWindowsHookEx")]
    private static extern IntPtr InternalSetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, int dwThreadId);

    /// <summary>
    /// Removes a hook procedure installed in a hook chain by the SetWindowsHookEx function.
    /// </summary>
    /// <param name="hHook">A handle to the hook to be removed.</param>
    /// <returns>true if successful, false otherwise.</returns>
    [DllImport("user32.dll", SetLastError = true, EntryPoint = "UnhookWindowsHookEx")]
    public static extern bool InternalUnhookWindowsHookEx(IntPtr hHook);

    /// <summary>
    /// Passes the hook information to the next hook procedure in the current hook chain.
    /// </summary>
    /// <param name="hHook">A handle to the current hook.</param>
    /// <param name="code">The hook code passed to the current hook procedure.</param>
    /// <param name="wParam">The wParam value passed to the current hook procedure.</param>
    /// <param name="lParam">The lParam value passed to the current hook procedure.</param>
    /// <returns>The return value of the next hook procedure.</returns>
    [DllImport("user32.dll", SetLastError = true, EntryPoint = "CallNextHookEx")]
    private static extern IntPtr InternalCallNextHookEx(IntPtr hHook, int code, IntPtr wParam, IntPtr lParam);

    /// <summary>
    /// Loads a dynamic-link library (DLL) into the address space of the calling process.
    /// </summary>
    /// <param name="lpFileName">The name of the DLL to load.</param>
    /// <returns>A handle to the loaded DLL if successful; otherwise, IntPtr.Zero.</returns>
    public IntPtr LoadLibrary(string lpFileName) => InternalLoadLibrary(lpFileName);

    /// <summary>
    /// Frees the loaded dynamic-link library (DLL).
    /// </summary>
    /// <param name="hModule">A handle to the loaded DLL.</param>
    /// <returns>true if successful, false otherwise.</returns>
    public bool FreeLibrary(IntPtr hModule) => InternalFreeLibrary(hModule);

    /// <summary>
    /// Installs an application-defined hook procedure into a hook chain.
    /// </summary>
    /// <param name="idHook">The type of hook to be installed.</param>
    /// <param name="lpfn">A pointer to the hook procedure.</param>
    /// <param name="hMod">A handle to the DLL containing the hook procedure.</param>
    /// <param name="dwThreadId">The identifier of the thread with which the hook procedure is to be associated.</param>
    /// <returns>A handle to the hook procedure if successful; otherwise, IntPtr.Zero.</returns>
    public IntPtr SetWindowsHookEx(int idHook, GlobalInputHook.HookProc lpfn, IntPtr hMod, int dwThreadId) => InternalSetWindowsHookEx(idHook, lpfn, hMod, dwThreadId);

    /// <summary>
    /// Removes a hook procedure installed in a hook chain by the SetWindowsHookEx function.
    /// </summary>
    /// <param name="hHook">A handle to the hook to be removed.</param>
    /// <returns>true if successful, false otherwise.</returns>
    public bool UnhookWindowsHookEx(IntPtr hHook) => InternalUnhookWindowsHookEx(hHook);

    /// <summary>
    /// Passes the hook information to the next hook procedure in the current hook chain.
    /// </summary>
    /// <param name="hHook">A handle to the current hook.</param>
    /// <param name="code">The hook code passed to the current hook procedure.</param>
    /// <param name="wParam">The wParam value passed to the current hook procedure.</param>
    /// <param name="lParam">The lParam value passed to the current hook procedure.</param>
    /// <returns>The return value of the next hook procedure.</returns>
    public IntPtr CallNextHookEx(IntPtr hHook, int code, IntPtr wParam, IntPtr lParam) => InternalCallNextHookEx(hHook, code, wParam, lParam);
}
