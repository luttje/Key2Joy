using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;

namespace Key2Joy.Mapping.Actions.Windows;

[Action(
    Description = "Get All Window Handles",
    Visibility = MappingMenuVisibility.Never,
    NameFormat = "Get All Window Handles"
)]
public class WindowGetAllAction : CoreAction
{
    public delegate bool EnumedWindow(IntPtr handleWindow, ArrayList handles);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool EnumWindows(EnumedWindow lpEnumFunc, ArrayList lParam);

    public WindowGetAllAction(string name)
        : base(name)
    { }

    public static ArrayList GetWindows()
    {
        ArrayList windowHandles = new();
        EnumedWindow callBackPtr = GetWindowHandle;
        EnumWindows(callBackPtr, windowHandles);

        return windowHandles;
    }

    private static bool GetWindowHandle(IntPtr windowHandle, ArrayList windowHandles)
    {
        windowHandles.Add(windowHandle);
        return true;
    }

    /// <markdown-doc>
    /// <parent-name>Windows</parent-name>
    /// <path>Api/Windows</path>
    /// </markdown-doc>
    /// <summary>
    /// Fetches all windows of software currently running.
    /// </summary>
    /// <returns>List with handles of all the windows</returns>
    /// <name>Window.GetAll</name>
    [ExposesScriptingMethod("Window.GetAll")]
    public object[] ExecuteForScript()
    {
        var windowHandles = GetWindows();
        return windowHandles.ToArray();
    }

    public override async Task Execute(AbstractInputBag inputBag = null)
    {
        // TODO: Currently this is only a script action...
    }

    public override string GetNameDisplay() => this.Name;

    public override bool Equals(object obj)
    {
        if (obj is not WindowGetAllAction)
        {
            return false;
        }

        // TODO: Currently this is only a script action so this is irrelevant
        return false;
    }
}
