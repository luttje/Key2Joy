using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using Key2Joy.LowLevelInput;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Key2Joy.Tests.Core.LowLevelInput;

public class MockHookManager : IHookManager
{
    public IntPtr LoadLibrary(string lpFileName) => new IntPtr(12345);  // Dummy value

    public bool FreeLibrary(IntPtr hModule) => true;

    public IntPtr SetWindowsHookEx(int idHook, GlobalInputHook.HookProc lpfn, IntPtr hMod, int dwThreadId) => new IntPtr(67890); // Another dummy value

    public bool UnhookWindowsHookEx(IntPtr hHook) => true;

    public IntPtr CallNextHookEx(IntPtr hHook, int code, IntPtr wParam, IntPtr lParam) => IntPtr.Zero;
}

[TestClass]
public class GlobalInputHookTests
{
    private GlobalInputHook globalInputHook;
    private MockHookManager mockHookManager;

    [TestInitialize]
    public void Setup()
    {
        this.mockHookManager = new MockHookManager();
        this.globalInputHook = new GlobalInputHook(this.mockHookManager);
    }

    private static IntPtr MockStructurePointer(object structure)
    {
        var pointer = Marshal.AllocHGlobal(Marshal.SizeOf(structure));
        Marshal.StructureToPtr(structure, pointer, false);

        return pointer;
    }

    [TestMethod]
    public void LowLevelInputHook_HandlesKeyboardInput()
    {
        var pointer = MockStructurePointer(new LowLevelKeyboardInputEvent());
        this.globalInputHook.KeyboardInputEvent += (sender, args) => args.Handled = true;  // Simulating that we're handling the event

        var result = this.globalInputHook.LowLevelInputHook(0, new IntPtr((int)KeyboardState.KeyDown), pointer);
        Assert.AreEqual(new IntPtr(1), result);
    }

    [TestMethod]
    public void LowLevelInputHook_HandlesMouseInput()
    {
        var pointer = MockStructurePointer(new LowLevelMouseInputEvent());
        this.globalInputHook.MouseInputEvent += (sender, args) => args.Handled = true;  // Simulating that we're handling the event

        var result = this.globalInputHook.LowLevelInputHook(0, new IntPtr((int)MouseState.LeftButtonDown), pointer);
        Assert.AreEqual(new IntPtr(1), result);
    }

    [TestMethod]
    public void LowLevelInputHook_PassesUnhandledKeyboardInput()
    {
        var pointer = MockStructurePointer(new LowLevelKeyboardInputEvent());
        var result = this.globalInputHook.LowLevelInputHook(0, new IntPtr((int)KeyboardState.KeyDown), pointer);
        Assert.AreEqual(IntPtr.Zero, result);
    }

    [TestMethod]
    public void LowLevelInputHook_PassesUnhandledMouseInput()
    {
        var pointer = MockStructurePointer(new LowLevelMouseInputEvent());
        var result = this.globalInputHook.LowLevelInputHook(0, new IntPtr((int)MouseState.LeftButtonDown), pointer);
        Assert.AreEqual(IntPtr.Zero, result);
    }
}
