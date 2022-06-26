using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KeyToJoy.Mapping
{
    [Action(
        Description = "Get Foreground Window Handle",
        Visibility = ActionVisibility.Never,
        NameFormat = "Get Foreground Window Handle"
    )]
    internal class WindowGetForegroundAction : BaseAction
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        public WindowGetForegroundAction(string name, string description)
            : base(name, description)
        { }

        /// <markdown-doc>
        /// <parent-name>Windows</parent-name>
        /// <path>Api</path>
        /// </markdown-doc>
        /// <summary>
        /// Get the handle of a software's window that is currently in the foreground.
        /// </summary>
        /// <returns>Handle for the window</returns>
        /// <name>WindowGetForeground</name>
        [ExposesScriptingMethod("WindowGetForeground")]
        public IntPtr ExecuteForScript()
        {
            var windowHandle = GetForegroundWindow();
            return windowHandle;
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
            if (!(obj is WindowFindAction action))
                return false;

            // TODO: Currently this is only a script action so this is irrelevant
            return false;
        }

        public override object Clone()
        {
            return new WindowFindAction(Name, description)
            {
                ImageResource = ImageResource,
                Name = Name,
            };
        }
    }
}
