using System;
using System.Runtime.InteropServices;
using System.Text;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;

namespace Key2Joy.Mapping.Actions.Windows;

[Action(
    Description = "Get Window Title",
    Visibility = MappingMenuVisibility.Never,
    NameFormat = "Get Window Title"
)]
public class WindowGetTitleAction : CoreAction
{
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern int GetWindowTextLength(IntPtr hWnd);

    public WindowGetTitleAction(string name)
        : base(name)
    { }

    /// <markdown-doc>
    /// <parent-name>Windows</parent-name>
    /// <path>Api/Windows</path>
    /// </markdown-doc>
    /// <summary>
    /// Get the title of a software's window.
    ///
    /// You can use <see href="Window.GetAll.md">Window.GetAll</see>, <see href="Window.Find.md">Window.Find</see> or <see href="Window.GetForeground.md">Window.GetForeground</see> to get handles.
    /// </summary>
    /// <param name="handle">The window handle to get the class for</param>
    /// <returns>Title of the window</returns>
    /// <name>Window.GetTitle</name>
    [ExposesScriptingMethod("Window.GetTitle")]
    public string ExecuteForScript(IntPtr handle)
    {
        var length = GetWindowTextLength(handle) + 1;
        StringBuilder title = new(length);
        GetWindowText(handle, title, length);
        return title.ToString();
    }

    public override string GetNameDisplay() => this.Name;

    public override bool Equals(object obj)
    {
        if (obj is not WindowGetTitleAction)
        {
            return false;
        }

        // TODO: Currently this is only a script action so this is irrelevant
        return false;
    }
}
