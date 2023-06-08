using Key2Joy.Contracts.Mapping;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    [Action(
        Description = "Get Foreground Window Handle",
        Visibility = MappingMenuVisibility.Never,
        NameFormat = "Get Foreground Window Handle"
    )]
    public class WindowGetForegroundAction : CoreAction
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        public WindowGetForegroundAction(string name)
            : base(name)
        { }

        /// <markdown-doc>
        /// <parent-name>Windows</parent-name>
        /// <path>Api/Windows</path>
        /// </markdown-doc>
        /// <summary>
        /// Get the handle of a software's window that is currently in the foreground.
        /// </summary>
        /// <returns>Handle for the window</returns>
        /// <name>Window.GetForeground</name>
        [ExposesScriptingMethod("Window.GetForeground")]
        public IntPtr ExecuteForScript()
        {
            var windowHandle = GetForegroundWindow();
            return windowHandle;
        }

        public override async Task Execute(AbstractInputBag inputBag = null)
        {
            // TODO: Currently this is only a script action...
        }

        public override string GetNameDisplay()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is WindowFindAction action))
                return false;

            // TODO: Currently this is only a script action so this is irrelevant
            return false;
        }
    }
}
