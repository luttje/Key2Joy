using Key2Joy.Contracts.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    [Action(
        Description = "Find Window Handle",
        Visibility = MappingMenuVisibility.Never,
        NameFormat = "Find Window '{0}' with title {1}"
    )]
    public class WindowFindAction : CoreAction
    {
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string className, string windowTitle);

        public string ClassName { get; set; }
        public string WindowName { get; set; }

        public WindowFindAction(string name)
            : base(name)
        { }

        /// <markdown-doc>
        /// <parent-name>Windows</parent-name>
        /// <path>Api/Windows</path>
        /// </markdown-doc>
        /// <summary>
        /// Find a window of a piece of software currently running.
        /// </summary>
        /// <param name="className">Window class name</param>
        /// <param name="windowTitle">Optional window title</param>
        /// <returns>Handle for the window</returns>
        /// <name>Window.Find</name>
        [ExposesScriptingMethod("Window.Find")]
        public IntPtr ExecuteForScript(string className, string windowTitle = null)
        {
            ClassName = className;
            WindowName = windowTitle;

            var windowHandle = FindWindow(ClassName, WindowName);
            return windowHandle;
        }

        public override async Task Execute(AbstractInputBag inputBag = null)
        {
            // TODO: Currently this is only a script action...
        }

        public override string GetNameDisplay()
        {
            return Name.Replace("{0}", ClassName)
                .Replace("{1}", WindowName);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is WindowFindAction action))
                return false;

            return action.ClassName == ClassName
                && action.WindowName == WindowName;
        }
    }
}
