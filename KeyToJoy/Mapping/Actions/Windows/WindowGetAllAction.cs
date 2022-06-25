using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KeyToJoy.Mapping
{
    [Action(
        Description = "Get All Window Handles",
        Visibility = ActionVisibility.Never,
        NameFormat = "Get All Window Handles",
        FunctionName = SCRIPT_COMMAND,
        FunctionMethodName = nameof(ExecuteActionForScript)
    )]
    internal class WindowGetAllAction : BaseAction
    {
        internal const string SCRIPT_COMMAND = "window_get_all";

        public delegate bool EnumedWindow(IntPtr handleWindow, ArrayList handles);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumWindows(EnumedWindow lpEnumFunc, ArrayList lParam);

        public WindowGetAllAction(string name, string description)
            : base(name, description)
        { }

        public static ArrayList GetWindows()
        {
            var windowHandles = new ArrayList();
            EnumedWindow callBackPtr = GetWindowHandle;
            EnumWindows(callBackPtr, windowHandles);

            return windowHandles;
        }

        private static bool GetWindowHandle(IntPtr windowHandle, ArrayList windowHandles)
        {
            windowHandles.Add(windowHandle);
            return true;
        }

        public object ExecuteActionForScript(BaseScriptAction scriptAction, params object[] parameters)
        {
            var windowHandles = GetWindows();
            return windowHandles.ToArray();
        }

        internal override async Task Execute(InputBag inputBag = null)
        {
            // TODO: Currently this is only a script action...
        }

        public override string GetNameDisplay()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is WindowGetAllAction action))
                return false;

            // TODO: Currently this is only a script action so this is irrelevant
            return false;
        }

        public override object Clone()
        {
            return new WindowGetAllAction(Name, description)
            {
                ImageResource = ImageResource,
                Name = Name,
            };
        }
    }
}
