using System;
using System.Runtime.InteropServices;
using System.Text;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;

namespace Key2Joy.Mapping.Actions.Windows;

[Action(
    Description = "Get Window Class name",
    Visibility = MappingMenuVisibility.Never,
    NameFormat = "Get Window Class name"
)]
public class WindowGetClassAction : CoreAction
{
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

    public WindowGetClassAction(string name)
        : base(name)
    { }

    /// <markdown-doc>
    /// <parent-name>Windows</parent-name>
    /// <path>Api/Windows</path>
    /// </markdown-doc>
    /// <summary>
    /// Get the class name for a specified Window.
    ///
    /// You can use <see href="Window.GetAllAction.md">Window.GetAllAction</see>, <see href="Window.FindAction.md">Window.FindAction</see> or  <see href="Window.GetForegroundAction.md">Window.GetForegroundAction</see> to get handles.
    /// </summary>
    /// <param name="handle">The window handle to get the class for</param>
    /// <returns>Class name for the window</returns>
    /// <name>Window.GetClass</name>
    [ExposesScriptingMethod("Window.GetClass")]
    public string ExecuteForScript(IntPtr handle)
    {
        int nRet;
        // Pre-allocate 256 characters, since this is the maximum class name length.
        StringBuilder classNameBuilder = new(256);
        //Get the window class name
        nRet = GetClassName(handle, classNameBuilder, classNameBuilder.Capacity);

        if (nRet == 0)
        {
            throw new InvalidCastException("Invalid window handle?");
        }

        return classNameBuilder.ToString();
    }

    public override string GetNameDisplay() => this.Name;

    public override bool Equals(object obj)
    {
        if (obj is not WindowGetClassAction)
        {
            return false;
        }

        // TODO: Currently this is only a script action so this is irrelevant
        return false;
    }
}
